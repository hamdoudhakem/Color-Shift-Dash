using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObsTypes
{
    public ObsTypes obsType { get; set; }
}

public enum ObsTypes {Doors , FallingBalls, BrickWall, SideWall, BallsPool,
                               Splasher, CannonsObs, ColBridge, Bridges, Rings, GroundSwitcher  }
