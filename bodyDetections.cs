using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data;
using System.Windows.Input;

namespace BodyBasicsWPF {
    
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using System.Windows.Forms;
    using WindowsInput;
    using WindowsInput.Native;

    public class detectArmForward {
    
        public void detectArmForward( JointType ShoulderPoint, JointType HandPoint ) {
            this.pointBase = ShoulderPoint;
            this.pointEnd  = HandPoint;
            this.direction = "negative";
            this.plane = "Z";
        }
    
    }

    public class checkNegativeDistanceMin {
        public void checkNegativeDistanceMin( JointType pointBase, JointType pointEnd, Joint.Position plane, int distanceMin ) {
		    this.pointBase = pointBase; 
            this.pointEnd = pointEnd; 
            this.plane = plane; 
            this.distanceMin = distanceMin;
        }
        public bool check() {
		    double handZ = joints[ this.pointEnd ].Position[ plane ];
		    double shouldZ = joints[ this.pointBase ].Position[ plane ];
		    double distance = (shouldZ - handZ) / 0.0254;
            return distance > distanceMine;
        }
    
    }

    public class checkPositiveDistanceMin {
        public void checkPositiveDistanceMin( pointBase, pointEnd, plane, distanceMin ) {
		    double handZ = joints[ pointEnd ].Position[ plane ];
		    double shouldZ = joints[ pointBase ].Position[ plane ];
		    double distance = (handZ - shouldZ) / 0.0254;
            return distance > distanceMine;
        }
    }

    public class executeKeyTap {

    }

    public class runActions  {
        /*/
        for( action in actionchecks ) {
            if( action.detector.checker.check() ) {
                action.executer.execute();
            }
        }
        /**/

    }
    
}



    /*/
class checkDistanceMin
class checkDistanceMax
class checkDistanceRange
class checkAngleMin
class checkAngleMax
class checkAngleRange

public class Class1
{
    setup a method for each type of body action.
    probably need a more central method for each type of distance/angle checks as well
    have config parser create a new instance of each?
     setup each detection as a different class with inheritance from a base class with the angle/distance check methods built in?
     how to setup an action set to check multiple requirements for a single execution?
     * have array of body instances with all same "checkok" style method with values set on initial loadConfig?

     * 
     * need the objects to have a reference to the base level joints somehow
    
    
    public bool detectArmForward() {

	}
}

class executeKeyTap
class executeKeyPress
class keyHold
class keyRelease
class mouseClickTap
class mouseClickHold
class mouseClickRelease
class mouseMove

or set these up as methods?
or not...

public addDetector( actionObject, detectType,  ) {

    switch( detectType ) {
        case "distanceMin":
        break;
    }

}
/**/
/*/
{
    sets : [
        {   
            bodyAction    : "handLeftForward", // always
            detectType    : "distanceMin",  // always
            distanceMin   : 12,
            executeAction : "keyType", // always
            executeChar   : "a"
         }
    ]
}
/**/
/*/
public loadconfig() {
    
    this.myActionChecks.Clear();
    configData = fileinfo("sf4.sdsds");
    detectionDirection = "";
    
    foreach( item in configData ) {
        
        newAction = new actionCheck();

        switch( item.bodyAction ) {
            case "handLeftForward":
                newAction.detector = new detectArmForward( JointTypes.ShoulderLeft, JointTypes.HandLeft );
            break;
        }

        switch( item.detectType ) {
            case "distanceMin":
                switch( newAction.detector.direction ) {
                    case "negative":
                        newAction.detector.checker = new checkNegativeDistanceMin( newAction.detector.pointBase, newAction.detector.pointEnd, newAction.detector.plane, item.distanceMin );
                    break;
                    case "positive":
                        newAction.detector.checker = new checkPositiveDistanceMin( newAction.detector.pointBase, newAction.detector.pointEnd, newAction.detector.plane, item.distanceMin );
                    break;
                }
            break;
        }
        
        switch( item.executeAction ) {
            case "keyTap":
                newAction.executer = new executeKeyTap( 123 );
            break;
        }

        this.myActionChecks[] = newAction;

    }

}
/**/