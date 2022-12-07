using System.Collections.Generic;
using System.Diagnostics;

namespace ImageGrabber.Core.CameraModule;

public class DummyCamera : CameraBase
{
    #region Constructor

    public DummyCamera()
    {
        
    }
    
    public DummyCamera(string name, string description) : base(name, description)
    {
    }

    #endregion
    
    public override void Open()
    {
    }

    public override void Close()
    {
        throw new System.NotImplementedException();
    }

    public override bool TryOpen()
    {
        throw new System.NotImplementedException();
    }

    public override void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public override bool ContinuousGrabbingMode { get; set; }
    public override void SetTriggerMode()
    {
        throw new System.NotImplementedException();
    }

    public override void SetParameters(Dictionary<string, object> dict)
    {
        throw new System.NotImplementedException();
    }

    public override void StartGrab()
    {
        throw new System.NotImplementedException();
    }

    public override void StopGrab()
    {
        throw new System.NotImplementedException();
    }
}