using System;
using System.Collections.Generic;

namespace ImageGrabber.Core.CameraModule;

/// <summary>
/// Abstract camera base inherited from <see cref="ICamera"/> interface
/// </summary>
public abstract class CameraBase : ICamera
{
    #region DelegateCommand

    public event GrabbedImageEventHandler OnGrabbedImageEvent;

    protected void OnGrabbedImageChanged(IGrabbedEvent e)
    {
        OnGrabbedImageEvent ?.Invoke(this, e);
    }
    
    #endregion
    
    #region Properties

    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsOpen { get; set; }
    public bool IsGrabbing { get; set; }
    public ECameraRotateType RotateType { get; set; }

    #endregion

    #region Constructor

    public CameraBase()
    {
        
    }

    protected CameraBase(string name, string description)
    {
        this.Name = name;
        this.Description = description;
        this.RotateType = ECameraRotateType.RotateNone;
    }

    #endregion

    #region Abstract Methods
    public abstract bool ContinuousGrabbingMode { get; set; }

    public abstract void Open();
    public virtual void Close()
    {
        IsOpen = false;
        RotateType = ECameraRotateType.RotateNone;
    }
    public abstract bool TryOpen();
    public abstract void Dispose();
    public abstract void SetTriggerMode();
    public abstract void SetParameters(Dictionary<string, object> dict);
    public virtual void StartGrab()
    {
        IsGrabbing = true;
    }
    public virtual void StopGrab()
    {
        IsGrabbing = false;
    }
    #endregion

    #region Public Methods
    public void SetRotation(ECameraRotateType rotateType)
    {
        this.RotateType = rotateType;
    }
    #endregion

}