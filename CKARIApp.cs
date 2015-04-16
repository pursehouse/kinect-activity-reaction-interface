public class CKARIApp
{
//C++ TO C# CONVERTER WARNING: The original C++ declaration of the following method implementation was not found:
	public void actionsDetect(bool bBlank, NUI_SKELETON_DATA pSkel, System.IntPtr hWnd, int skeletonNumber)
	{
    
		if (!checkSkeltonValid(pSkel) || !bodySegmentsMeasure(pSkel))
		{
			return;
		}

		SortedDictionary<int, string> curBodyParts = new SortedDictionary<int, string>();
		SortedDictionary<int, int> curJoints = new SortedDictionary<int, int>();
		SortedDictionary<int, float> jointDistances = new SortedDictionary<int, float>();
		SortedDictionary<int, float> jointAnglesX = new SortedDictionary<int, float>();
		SortedDictionary<int, float> jointAnglesY = new SortedDictionary<int, float>();
    
		bool go;
		int distance;
		double angleX;
		double angleY;
		float leftX;
		float centerX;
		float rightX;
		int actionSetNum;
    
		for (uint i = 0; i < actionsCount; i++)
		{
    
			go = true;
			distance = 0;
			angleX = 0;
			angleY = 0;
			leftX = 0F;
			centerX = 0F;
			rightX = 0F;
			actionSetNum = 0;
    
			curJoints.Clear();
			jointDistances.Clear();
			curBodyParts.Clear();
    
			for (uint i2 = 0; i2 < actionSets[i][AS_REQUIRE].Count; i2++)
			{
    
				curBodyParts[curBodyParts.Count] = s_bodyActions[actionSets[i][AS_REQUIRE][i2][AS_BODY_ACTION]];
    
				switch (actionSets[i][AS_REQUIRE][i2][AS_BODY_ACTION])
				{
					case BA_LEAN_LEFT: //    lean_left                :    angular body lean left (degrees)
					break;
					case BA_LEAN_RIGHT: //    lean_right                :    angular body lean right(degrees)
					break;
					case BA_LEAN_FORWARD: //    lean_forwards            :    angualr body lean forwards (degrees)
					break;
					case BA_LEAN_BACKWARD: //    lean_backwards            :    angular body lean back (degrees)
					break;
					case BA_TURN_LEFT: //    turn_left                :    angular amount of left body turn (degrees)
						curJoints[curJoints.Count] = NUI_JOINT_SHOULDER_LEFT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_ANGLE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& centerZ = pSkel->joints[NUI_JOINT_SHOULDER_CENTER].z;
							float centerZ = pSkel.joints[NUI_JOINT_SHOULDER_CENTER].z;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& rightZ = pSkel->joints[NUI_JOINT_SHOULDER_RIGHT].z;
							float rightZ = pSkel.joints[NUI_JOINT_SHOULDER_RIGHT].z;
							if (centerZ > rightZ)
							{
								centerX = pSkel.joints[NUI_JOINT_SHOULDER_CENTER].x;
								rightX = pSkel.joints[NUI_JOINT_SHOULDER_RIGHT].x;
								if (centerX < rightX)
								{
									angleX = Math.Atan2((double)(centerZ - rightZ), (double)(centerX - rightX)) * 180 / DefineConstants.PI;
								}
							}
						}
					break;
					case BA_TURN_RIGHT: //    turn_right                :    angular amount of right body turn(degrees)
						curJoints[curJoints.Count] = NUI_JOINT_SHOULDER_RIGHT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_ANGLE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& centerZ = pSkel->joints[NUI_JOINT_SHOULDER_CENTER].z;
							float centerZ = pSkel.joints[NUI_JOINT_SHOULDER_CENTER].z;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& leftZ = pSkel->joints[NUI_JOINT_SHOULDER_LEFT].z;
							float leftZ = pSkel.joints[NUI_JOINT_SHOULDER_LEFT].z;
							if (centerZ > leftZ)
							{
								centerX = pSkel.joints[NUI_JOINT_SHOULDER_CENTER].x;
								leftX = pSkel.joints[NUI_JOINT_SHOULDER_LEFT].x;
								if (centerX > leftX)
								{
									angleX = Math.Atan2((double)(centerZ - leftZ), (double)(centerX - leftX)) * 180 / DefineConstants.PI;
								}
							}
						}
					break;
					case BA_HAND_LEFT_FORWARD: //    left_arm_forwards        :    forward distance from left hand to shoulder (inches)
						curJoints[curJoints.Count] = NUI_JOINT_HAND_LEFT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handZ = pSkel->joints[NUI_JOINT_HAND_LEFT].z;
							float handZ = pSkel.joints[NUI_JOINT_HAND_LEFT].z;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldZ = pSkel->joints[NUI_JOINT_SHOULDER_LEFT].z;
							float shouldZ = pSkel.joints[NUI_JOINT_SHOULDER_LEFT].z;
							jointDistances[NUI_JOINT_HAND_LEFT] = (shouldZ - handZ) / 0.0254;
								distance = (int)jointDistances[NUI_JOINT_HAND_LEFT];
						}
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_ANGLE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handX = pSkel->joints[NUI_JOINT_HAND_LEFT].x;
							float handX = pSkel.joints[NUI_JOINT_HAND_LEFT].x;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handY = pSkel->joints[NUI_JOINT_HAND_LEFT].y;
							float handY = pSkel.joints[NUI_JOINT_HAND_LEFT].y;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handZ = pSkel->joints[NUI_JOINT_HAND_LEFT].z;
							float handZ = pSkel.joints[NUI_JOINT_HAND_LEFT].z;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldX = pSkel->joints[NUI_JOINT_SHOULDER_LEFT].x;
							float shouldX = pSkel.joints[NUI_JOINT_SHOULDER_LEFT].x;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldY = pSkel->joints[NUI_JOINT_SHOULDER_LEFT].y;
							float shouldY = pSkel.joints[NUI_JOINT_SHOULDER_LEFT].y;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldZ = pSkel->joints[NUI_JOINT_SHOULDER_LEFT].z;
							float shouldZ = pSkel.joints[NUI_JOINT_SHOULDER_LEFT].z;
									angleX = Math.Atan2((double)(shouldZ - handZ), (double)(shouldX - handX)) * 180 / DefineConstants.PI;
									angleY = Math.Atan2((double)(shouldZ - handZ), (double)(shouldY - handY)) * 180 / DefineConstants.PI;
							jointAnglesX[NUI_JOINT_HAND_LEFT] = angleX;
							jointAnglesY[NUI_JOINT_HAND_LEFT] = angleY;
						}
					break;
					case BA_HAND_LEFT_DOWN: //    left_arm_down            :    downward distance from left hand to shoulder (inches)
						curJoints[curJoints.Count] = NUI_JOINT_HAND_LEFT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handY = pSkel->joints[NUI_JOINT_HAND_LEFT].y;
							float handY = pSkel.joints[NUI_JOINT_HAND_LEFT].y;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldY = pSkel->joints[NUI_JOINT_SHOULDER_LEFT].y;
							float shouldY = pSkel.joints[NUI_JOINT_SHOULDER_LEFT].y;
									distance = (int)((shouldY - handY) / 0.0254);
						}
					break;
					case BA_HAND_LEFT_UP: //    left_arm_up                :    upward distance from left hand to shoulder (inches)
						curJoints[curJoints.Count] = NUI_JOINT_HAND_LEFT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handY = pSkel->joints[NUI_JOINT_HAND_LEFT].y;
							float handY = pSkel.joints[NUI_JOINT_HAND_LEFT].y;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldY = pSkel->joints[NUI_JOINT_SHOULDER_LEFT].y;
							float shouldY = pSkel.joints[NUI_JOINT_SHOULDER_LEFT].y;
							if (handY > shouldY)
							{
									distance = (int)((handY - shouldY) / 0.0254);
							}
						}
					break;
					case BA_HAND_LEFT_OUT: //    left_arm_out            :    sideways distance from left hand to shoulder (inches)
						curJoints[curJoints.Count] = NUI_JOINT_HAND_LEFT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handX = pSkel->joints[NUI_JOINT_HAND_LEFT].x;
							float handX = pSkel.joints[NUI_JOINT_HAND_LEFT].x;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldX = pSkel->joints[NUI_JOINT_SHOULDER_LEFT].x;
							float shouldX = pSkel.joints[NUI_JOINT_SHOULDER_LEFT].x;
							if (handX < shouldX)
							{
								distance = (int)((shouldX - handX) / 0.0254);
							}
						}
					break;
					case BA_HAND_LEFT_ACROSS: //    left_arm_across            :    sideways distance from left hand across body to shoulder (inches)
						curJoints[curJoints.Count] = NUI_JOINT_HAND_LEFT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handX = pSkel->joints[NUI_JOINT_HAND_LEFT].x;
							float handX = pSkel.joints[NUI_JOINT_HAND_LEFT].x;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldX = pSkel->joints[NUI_JOINT_SHOULDER_LEFT].x;
							float shouldX = pSkel.joints[NUI_JOINT_SHOULDER_LEFT].x;
							if (handX > shouldX)
							{
								distance = (int)((handX - shouldX) / 0.0254);
							}
						}
					break;
					case BA_HAND_RIGHT_FORWARD: //    right_arm_forwards        :    forward distance from right hand to shoulder (inches)
						curJoints[curJoints.Count] = NUI_JOINT_HAND_RIGHT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handZ = pSkel->joints[NUI_JOINT_HAND_RIGHT].z;
							float handZ = pSkel.joints[NUI_JOINT_HAND_RIGHT].z;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldZ = pSkel->joints[NUI_JOINT_SHOULDER_RIGHT].z;
							float shouldZ = pSkel.joints[NUI_JOINT_SHOULDER_RIGHT].z;
							jointDistances[NUI_JOINT_HAND_RIGHT] = (shouldZ - handZ) / 0.0254;
									distance = (int)jointDistances[NUI_JOINT_HAND_RIGHT];
						}
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_ANGLE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handX = pSkel->joints[NUI_JOINT_HAND_RIGHT].x;
							float handX = pSkel.joints[NUI_JOINT_HAND_RIGHT].x;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handY = pSkel->joints[NUI_JOINT_HAND_RIGHT].y;
							float handY = pSkel.joints[NUI_JOINT_HAND_RIGHT].y;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handZ = pSkel->joints[NUI_JOINT_HAND_RIGHT].z;
							float handZ = pSkel.joints[NUI_JOINT_HAND_RIGHT].z;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldX = pSkel->joints[NUI_JOINT_SHOULDER_RIGHT].x;
							float shouldX = pSkel.joints[NUI_JOINT_SHOULDER_RIGHT].x;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldY = pSkel->joints[NUI_JOINT_SHOULDER_RIGHT].y;
							float shouldY = pSkel.joints[NUI_JOINT_SHOULDER_RIGHT].y;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldZ = pSkel->joints[NUI_JOINT_SHOULDER_RIGHT].z;
							float shouldZ = pSkel.joints[NUI_JOINT_SHOULDER_RIGHT].z;
									angleX = Math.Atan2((double)(shouldZ - handZ), (double)(shouldX - handX)) * 180 / DefineConstants.PI;
									angleY = Math.Atan2((double)(shouldZ - handZ), (double)(shouldY - handY)) * 180 / DefineConstants.PI;
							jointAnglesX[NUI_JOINT_HAND_RIGHT] = angleX;
							jointAnglesY[NUI_JOINT_HAND_RIGHT] = angleY;
						}
					break;
					case BA_HAND_RIGHT_DOWN: //    right_arm_down            :    downward distance from right hand to shoulder (inches)
						curJoints[curJoints.Count] = NUI_JOINT_HAND_RIGHT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handY = pSkel->joints[NUI_JOINT_HAND_RIGHT].y;
							float handY = pSkel.joints[NUI_JOINT_HAND_RIGHT].y;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldY = pSkel->joints[NUI_JOINT_SHOULDER_RIGHT].y;
							float shouldY = pSkel.joints[NUI_JOINT_SHOULDER_RIGHT].y;
									distance = (int)((shouldY - handY) / 0.0254);
						}
					break;
					case BA_HAND_RIGHT_UP: //    right_arm_up            :    upward distance from right hand to shoulder (inches)
						curJoints[curJoints.Count] = NUI_JOINT_HAND_RIGHT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handY = pSkel->joints[NUI_JOINT_HAND_RIGHT].y;
							float handY = pSkel.joints[NUI_JOINT_HAND_RIGHT].y;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldY = pSkel->joints[NUI_JOINT_SHOULDER_RIGHT].y;
							float shouldY = pSkel.joints[NUI_JOINT_SHOULDER_RIGHT].y;
							if (handY > shouldY)
							{
								distance = (int)((handY - shouldY) / 0.0254);
							}
						}
					break;
					case BA_HAND_RIGHT_OUT: //    right_arm_out            :    sideways distance from right hand to shoulder (inches)
						curJoints[curJoints.Count] = NUI_JOINT_HAND_RIGHT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handX = pSkel->joints[NUI_JOINT_HAND_RIGHT].x;
							float handX = pSkel.joints[NUI_JOINT_HAND_RIGHT].x;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldX = pSkel->joints[NUI_JOINT_SHOULDER_RIGHT].x;
							float shouldX = pSkel.joints[NUI_JOINT_SHOULDER_RIGHT].x;
							if (handX > shouldX)
							{
								distance = (int)((handX - shouldX) / 0.0254);
							}
						}
					break;
					case BA_HAND_RIGHT_ACROSS: //    right_arm_across        :    sideways distance from right hand across body to shoulder (inches)
						curJoints[curJoints.Count] = NUI_JOINT_HAND_RIGHT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handX = pSkel->joints[NUI_JOINT_HAND_RIGHT].x;
							float handX = pSkel.joints[NUI_JOINT_HAND_RIGHT].x;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldX = pSkel->joints[NUI_JOINT_SHOULDER_RIGHT].x;
							float shouldX = pSkel.joints[NUI_JOINT_SHOULDER_RIGHT].x;
							if (handX < shouldX)
							{
								distance = (int)((shouldX - handX) / 0.0254);
							}
						}
					break;
					case BA_FOOT_LEFT_FORWARD: //    left_foot_forwards        :    forward distance from left hip to foot (inches)
						curJoints[curJoints.Count] = NUI_JOINT_FOOT_LEFT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& footZ = pSkel->joints[NUI_JOINT_FOOT_LEFT].z;
							float footZ = pSkel.joints[NUI_JOINT_FOOT_LEFT].z;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& hipZ = pSkel->joints[NUI_JOINT_HIP_LEFT].z;
							float hipZ = pSkel.joints[NUI_JOINT_HIP_LEFT].z;
									distance = (int)((hipZ - footZ) / 0.0254);
						}
					break;
					case BA_FOOT_LEFT_OUT: //    left_foot_sideways        :    sideways distance from left hip to foot (inches)
						curJoints[curJoints.Count] = NUI_JOINT_FOOT_LEFT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& footX = pSkel->joints[NUI_JOINT_FOOT_LEFT].x;
							float footX = pSkel.joints[NUI_JOINT_FOOT_LEFT].x;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& hipX = pSkel->joints[NUI_JOINT_HIP_LEFT].x;
							float hipX = pSkel.joints[NUI_JOINT_HIP_LEFT].x;
							if (footX < hipX)
							{
								distance = (int)((hipX - footX) / 0.0254);
							}
						}
					break;
					case BA_FOOT_LEFT_ACROSS: //    right_arm_across        :    sideways distance from right hand across body to shoulder (inches)
						curJoints[curJoints.Count] = NUI_JOINT_FOOT_LEFT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& footX = pSkel->joints[NUI_JOINT_FOOT_LEFT].x;
							float footX = pSkel.joints[NUI_JOINT_FOOT_LEFT].x;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& hipX = pSkel->joints[NUI_JOINT_HIP_LEFT].x;
							float hipX = pSkel.joints[NUI_JOINT_HIP_LEFT].x;
							if (footX > hipX)
							{
								distance = (int)((footX - hipX) / 0.0254);
							}
						}
					break;
					case BA_FOOT_LEFT_BACKWARD: //    left_foot_backwards        :    backwards distance from left hip to foot (inches)
					break;
					case BA_FOOT_LEFT_UP: //    left_foot_up            :    height of left foot above other foot on ground (inches)
						curJoints[curJoints.Count] = NUI_JOINT_FOOT_LEFT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& leftY = pSkel->joints[NUI_JOINT_FOOT_LEFT].y;
							float leftY = pSkel.joints[NUI_JOINT_FOOT_LEFT].y;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& rightY = pSkel->joints[NUI_JOINT_FOOT_RIGHT].y;
							float rightY = pSkel.joints[NUI_JOINT_FOOT_RIGHT].y;
							if (leftY > rightY)
							{
								distance = (int)((leftY - rightY) / 0.0254);
							}
						}
					break;
					case BA_FOOT_RIGHT_FORWARD: //    right_foot_forwards        :    forward distance from right hip to foot (inches)
						curJoints[curJoints.Count] = NUI_JOINT_FOOT_RIGHT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& footZ = pSkel->joints[NUI_JOINT_FOOT_RIGHT].z;
							float footZ = pSkel.joints[NUI_JOINT_FOOT_RIGHT].z;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& hipZ = pSkel->joints[NUI_JOINT_HIP_RIGHT].z;
							float hipZ = pSkel.joints[NUI_JOINT_HIP_RIGHT].z;
									distance = (int)((hipZ - footZ) / 0.0254);
						}
					break;
					case BA_FOOT_RIGHT_OUT: //    right_foot_sideways        :    sideways distance from right hip to foot (inches)
						curJoints[curJoints.Count] = NUI_JOINT_FOOT_RIGHT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& footX = pSkel->joints[NUI_JOINT_FOOT_RIGHT].x;
							float footX = pSkel.joints[NUI_JOINT_FOOT_RIGHT].x;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& hipX = pSkel->joints[NUI_JOINT_HIP_RIGHT].x;
							float hipX = pSkel.joints[NUI_JOINT_HIP_RIGHT].x;
							if (footX > hipX)
							{
								distance = (int)((footX - hipX) / 0.0254);
							}
						}
					break;
					case BA_FOOT_RIGHT_ACROSS: //    right_arm_across        :    sideways distance from right hand across body to shoulder (inches)
						curJoints[curJoints.Count] = NUI_JOINT_FOOT_RIGHT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& footX = pSkel->joints[NUI_JOINT_FOOT_RIGHT].x;
							float footX = pSkel.joints[NUI_JOINT_FOOT_RIGHT].x;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& hipX = pSkel->joints[NUI_JOINT_HIP_RIGHT].x;
							float hipX = pSkel.joints[NUI_JOINT_HIP_RIGHT].x;
							if (footX < hipX)
							{
								distance = (int)((hipX - footX) / 0.0254);
							}
						}
					break;
					case BA_FOOT_RIGHT_BACKWARD: //    right_foot_backwards    :    backwards distance from right hip to foot (inches)
					break;
					case BA_FOOT_RIGHT_UP: //    right_foot_up            :    height of right foot above other foot on ground (inches)
						curJoints[curJoints.Count] = NUI_JOINT_FOOT_RIGHT;
						if (actionSets[i][AS_REQUIRE][i2][AS_USE_DISTANCE] != 0)
						{
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& leftY = pSkel->joints[NUI_JOINT_FOOT_LEFT].y;
							float leftY = pSkel.joints[NUI_JOINT_FOOT_LEFT].y;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& rightY = pSkel->joints[NUI_JOINT_FOOT_RIGHT].y;
							float rightY = pSkel.joints[NUI_JOINT_FOOT_RIGHT].y;
							if (rightY > leftY)
							{
								distance = (int)((rightY - leftY) / 0.0254);
							}
						}
					break;
					case BA_JUMP: //    jump                    :    height of both feet above ground (inches)
						curJoints[curJoints.Count] = NUI_JOINT_FOOT_RIGHT;
						curJoints[curJoints.Count] = NUI_JOINT_FOOT_LEFT;
    
						distance = getSkeletonElevation(pSkel);
    
						SetDlgItemInt(m_hWnd, IDC_PERSON_ELEVATION, distance, 0);
    
					break;
					case BA_CROUCH: //    crouch                    :    crouch distance: calculated as current height subtracted from standing height (inches)
    
						if (getSkeletonElevation(pSkel) < 10)
						{
    
							curJoints[curJoints.Count] = NUI_JOINT_HEAD;
    
							float footY;
    
							footY = pSkel.joints[NUI_JOINT_FOOT_LEFT].y < pSkel.joints[NUI_JOINT_FOOT_RIGHT].y ? pSkel.joints[NUI_JOINT_FOOT_LEFT].y : pSkel.joints[NUI_JOINT_FOOT_RIGHT].y;
    
							int curHeight = (pSkel.joints[NUI_JOINT_HEAD].y - footY) / 0.0254;
    
							if (personHeight > curHeight)
							{
								distance = personHeight - curHeight;
							}
    
						}
    
					break;
					case BA_WALK: //    walk                    :    height of each step above ground when walking in place (inches)
						curJoints[curJoints.Count] = NUI_JOINT_FOOT_RIGHT;
						curJoints[curJoints.Count] = NUI_JOINT_FOOT_LEFT;
					break;
				}
    
						if (actionSets[i][AS_REQUIRE][i2][AS_DISTANCE_MIN] != 0 && distance < actionSets[i][AS_REQUIRE][i2][AS_DISTANCE_MIN])
						{
							go = false;
							break;
						}
				else if (actionSets[i][AS_REQUIRE][i2][AS_DISTANCE_MAX] != 0 && distance > actionSets[i][AS_REQUIRE][i2][AS_DISTANCE_MAX])
				{
					go = false;
					break;
				}
				else if (actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_X] != 0 && angleX < actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_X])
				{
					go = false;
					break;
				}
				else if (actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MAX_X] != 0 && angleX > actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MAX_X])
				{
					go = false;
					break;
				}
				else if (actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_Y] != 0 && angleY < actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_Y])
				{
					go = false;
					break;
				}
				else if (actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MAX_Y] != 0 && angleY > actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MAX_Y])
				{
					go = false;
					break;
				}
    
			}
    
			if (go == true)
			{
    
				//NUI_SKELETON_DATA * pSkel
				NUI_SKELETON_DATA prevSkel = previousSkeltonFrame.SkeletonData[skeletonNumber];
    
				for (uint i2 = 0; i2 < actionSets[i][AS_EXECUTE].Count; i2++)
				{
    
					if (actionSets[i][AS_EXECUTE][i2][AS_MOUSE_TRACK] != 0 || actionSets[i][AS_EXECUTE][i2][AS_WINDOW_HOLD] != 0)
					{
    
						float dDiff = jointDistances[curJoints[0]] - actionSets[i][AS_REQUIRE][i2][AS_DISTANCE_MIN];
						float dRange = actionSets[i][AS_REQUIRE][i2][AS_DISTANCE_MAX] - actionSets[i][AS_REQUIRE][i2][AS_DISTANCE_MIN];
						float dStrength = dDiff / dRange;
    
						// get should point from hand point
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldX = pSkel->joints[curJoints[0] - 3].x;
						float shouldX = pSkel.joints[curJoints[0] - 3].x;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldY = pSkel->joints[curJoints[0] - 3].y;
						float shouldY = pSkel.joints[curJoints[0] - 3].y;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& shouldZ = pSkel->joints[curJoints[0] - 3].z;
						float shouldZ = pSkel.joints[curJoints[0] - 3].z;
    
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handX = pSkel->joints[curJoints[0]].x;
						float handX = pSkel.joints[curJoints[0]].x;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handY = pSkel->joints[curJoints[0]].y;
						float handY = pSkel.joints[curJoints[0]].y;
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for references to value types:
	//ORIGINAL LINE: float& handZ = pSkel->joints[curJoints[0]].z;
						float handZ = pSkel.joints[curJoints[0]].z;
    
						int offsetX;
						int offsetY;
    
						int xGoal;
						int yGoal;
						float xDiff;
						float yDiff;
						float yRange;
						float xPercent;
						float yPercent;
						float xRange;
						float xAngle;
						float yAngle;
						float prevAngleX;
						float prevAngleY;
						float anglePixelsX;
						float anglePixelsY;
    
						Vector4 prevShould = prevSkel.joints[curJoints[0] - 3];
						Vector4 prevHand = prevSkel.joints[curJoints[0]];
    
						switch (actionSets[i][AS_EXECUTE][i2][AS_MOUSE_TRACK])
						{
							case ACV_ABSOLUTE:
								xAngle = jointAnglesX[curJoints[0]];
								yAngle = jointAnglesY[curJoints[0]];
    
								xRange = actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MAX_X] - actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_X];
								xDiff = xAngle - actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_X];
								xPercent = xDiff / xRange;
    
								yRange = actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MAX_Y] - actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_Y];
								yDiff = yAngle - actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_Y];
								yPercent = yDiff / yRange;
    
								xGoal = screenSizeX * xPercent;
								yGoal = screenSizeY * (1 - yPercent);
    
								SetCursorPos(xGoal, yGoal);
							break;
							case ACV_PUSH:
								offsetX = (int)(((handX - shouldX) / 0.0254) / dStrength);
								offsetY = (int)(((shouldY - handY) / 0.0254) / dStrength);
								if (offsetX != 0 || offsetY != 0)
								{
									mouseMove(offsetX, offsetY);
								}
							break;
							case ACV_RELATIVE:
								/*/
								setup so that movement range is mapped to detection area and screen size, same as absolute.
								/**/
    
								prevAngleX = Math.Atan2((double)(prevShould.z - prevHand.z), (double)(prevShould.x - prevHand.x)) * 180 / DefineConstants.PI;
								prevAngleY = Math.Atan2((double)(prevShould.z - prevHand.z), (double)(prevShould.y - prevHand.y)) * 180 / DefineConstants.PI;
    
								xRange = actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MAX_X] - actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_X];
								yRange = actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MAX_Y] - actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_Y];
    
								anglePixelsX = screenSizeX / xRange;
								anglePixelsY = screenSizeY / yRange;
    
								xAngle = jointAnglesX[curJoints[0]];
								yAngle = jointAnglesY[curJoints[0]];
    
								offsetX = (xAngle - prevAngleX) * anglePixelsX;
								offsetY = (prevAngleY - yAngle) * anglePixelsY;
    
								if (offsetX != 0 || offsetY != 0)
								{
									//m_Log << "dragx : " << xAngle << "\t" << prevAngleX << "\t" << (xAngle - prevAngleX) << "\t" << anglePixelsX << "\t" << offsetX << "\n";
									//m_Log << "dragy : " << yAngle << "\t" << prevAngleY << "\t" << (yAngle - prevAngleY) << "\t" << anglePixelsY << "\t" << offsetY << "\n";
									mouseMove(offsetX, offsetY);
								}
    
							break;
							case ACV_DRAG:
								handX /= 0.0254;
								handY /= 0.0254;
								handX = (int)handX;
								handY = (int)handY;
								if (actionSets[i][AS_EXECUTE][i2][AS_JOINT_PREV_Y] == null)
								{
									actionSets[i][AS_EXECUTE][i2][AS_JOINT_PREV_X] = handX;
									actionSets[i][AS_EXECUTE][i2][AS_JOINT_PREV_Y] = handY;
								}
								else
								{
									offsetX = handX - actionSets[i][AS_EXECUTE][i2][AS_JOINT_PREV_X];
									offsetY = actionSets[i][AS_EXECUTE][i2][AS_JOINT_PREV_Y] - handY;
									if (offsetX != 0 || offsetY != 0)
									{
										if (dStrength != 0)
										{
											if (dStrength < .02)
											{
												dStrength = .02F;
											}
											if (offsetX != 0)
											{
												offsetX /= dStrength;
											}
											if (offsetY != 0)
											{
												offsetY /= dStrength;
											}
										}
										mouseMove(offsetX, offsetY);
										actionSets[i][AS_EXECUTE][i2][AS_JOINT_PREV_X] = handX;
										actionSets[i][AS_EXECUTE][i2][AS_JOINT_PREV_Y] = handY;
									}
								}
							break;
						}
    
						switch (actionSets[i][AS_EXECUTE][i2][AS_WINDOW_HOLD])
						{
							case ACV_ABSOLUTE:
								xAngle = jointAnglesX[curJoints[0]];
								yAngle = jointAnglesY[curJoints[0]];
    
								xRange = actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MAX_X] - actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_X];
								xDiff = xAngle - actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_X];
								xPercent = xDiff / xRange;
    
								yRange = actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MAX_Y] - actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_Y];
								yDiff = yAngle - actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_Y];
								yPercent = yDiff / yRange;
    
								xGoal = screenSizeX * xPercent;
								yGoal = screenSizeY * (1 - yPercent);
    
								placeActiveWindow(xGoal, yGoal);
							break;
							case ACV_PUSH:
								offsetX = (int)(((handX - shouldX) / 0.0254) / dStrength);
								offsetY = (int)(((shouldY - handY) / 0.0254) / dStrength);
								if (offsetX != 0 || offsetY != 0)
								{
									moveActiveWindow(offsetX, offsetY);
								}
							break;
							case ACV_RELATIVE:
    
								prevAngleX = Math.Atan2((double)(prevShould.z - prevHand.z), (double)(prevShould.x - prevHand.x)) * 180 / DefineConstants.PI;
								prevAngleY = Math.Atan2((double)(prevShould.z - prevHand.z), (double)(prevShould.y - prevHand.y)) * 180 / DefineConstants.PI;
    
								xRange = actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MAX_X] - actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_X];
								yRange = actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MAX_Y] - actionSets[i][AS_REQUIRE][i2][AS_ANGLE_MIN_Y];
    
								anglePixelsX = screenSizeX / xRange;
								anglePixelsY = screenSizeY / yRange;
    
								xAngle = jointAnglesX[curJoints[0]];
								yAngle = jointAnglesY[curJoints[0]];
    
								offsetX = (xAngle - prevAngleX) * anglePixelsX;
								offsetY = (prevAngleY - yAngle) * anglePixelsY;
    
								if (offsetX != 0 || offsetY != 0)
								{
									//m_Log << "dragx : " << xAngle << "\t" << prevAngleX << "\t" << (xAngle - prevAngleX) << "\t" << anglePixelsX << "\t" << offsetX << "\n";
									//m_Log << "dragy : " << yAngle << "\t" << prevAngleY << "\t" << (yAngle - prevAngleY) << "\t" << anglePixelsY << "\t" << offsetY << "\n";
									moveActiveWindow(offsetX, offsetY);
								}
							break;
							case ACV_DRAG:
								handX /= 0.0254;
								handY /= 0.0254;
								handX = (int)handX;
								handY = (int)handY;
								if (actionSets[i][AS_EXECUTE][i2][AS_JOINT_PREV_Y] == null)
								{
									actionSets[i][AS_EXECUTE][i2][AS_JOINT_PREV_X] = handX;
									actionSets[i][AS_EXECUTE][i2][AS_JOINT_PREV_Y] = handY;
								}
								else
								{
									offsetX = handX - actionSets[i][AS_EXECUTE][i2][AS_JOINT_PREV_X];
									offsetY = actionSets[i][AS_EXECUTE][i2][AS_JOINT_PREV_Y] - handY;
									if (offsetX != 0 || offsetY != 0)
									{
										if (dStrength != 0)
										{
											if (dStrength < .02)
											{
												dStrength = .02F;
											}
											if (offsetX != 0)
											{
												offsetX /= dStrength;
											}
											if (offsetY != 0)
											{
												offsetY /= dStrength;
											}
										}
										moveActiveWindow(offsetX, offsetY);
										actionSets[i][AS_EXECUTE][i2][AS_JOINT_PREV_X] = handX;
										actionSets[i][AS_EXECUTE][i2][AS_JOINT_PREV_Y] = handY;
									}
								}
							break;
						}
    
					}
					else
					{
					if (!actionSetsStatus[i])
					{
    
						//m_Log << "on : " << (int)i << "\n";
						for (int cb = 0; cb < curBodyParts.Count; cb++)
						{
							//m_Log << "\t" << curBodyParts[cb] << "\n";
						}
						//m_Log << "\t";
						//m_Log << "\td:" << distance;
						//m_Log << "\taX:" << angleX;
						//m_Log << "\taY:" << angleY;
						//m_Log << "\n";
    
						if (actionSets[i][AS_EXECUTE][i2][AS_KEY_TAP] != 0)
						{
							//m_Log << "keyTap : " << actionSets[i][AS_EXECUTE][i2][AS_KEY_TAP] << "\n";
							_beginthread(keyTap, 0, (object)actionSets[i][AS_EXECUTE][i2][AS_KEY_TAP]);
						}
    
						if (actionSets[i][AS_EXECUTE][i2][AS_KEY_HOLD] != 0)
						{
							//m_Log << "keyPress : " << actionSets[i][AS_EXECUTE][i2][AS_KEY_HOLD] << "\n";
							_beginthread(keyPress, 0, (object)actionSets[i][AS_EXECUTE][i2][AS_KEY_HOLD]);
						}
    
						if (actionSets[i][AS_EXECUTE][i2][AS_KEY_PRESS] != 0)
						{
							//m_Log << "keyPress : " << actionSets[i][AS_EXECUTE][i2][AS_KEY_PRESS] << "\n";
							_beginthread(keyPress, 0, (object)actionSets[i][AS_EXECUTE][i2][AS_KEY_PRESS]);
						}
    
						if (actionSets[i][AS_EXECUTE][i2][AS_KEY_RELEASE] != 0)
						{
							//m_Log << "keyRelease : " << actionSets[i][AS_EXECUTE][i2][AS_KEY_RELEASE] << "\n";
							_beginthread(keyRelease, 0, (object)actionSets[i][AS_EXECUTE][i2][AS_KEY_RELEASE]);
						}
    
						if (actionSets[i][AS_EXECUTE][i2][AS_MOUSE_TAP] != 0)
						{
    
							int down;
							int up;
							switch (actionSets[i][AS_EXECUTE][i2][AS_MOUSE_TAP])
							{
								case ACV_MOUSE_LEFT:
									down = MOUSEEVENTF_LEFTDOWN;
									up = MOUSEEVENTF_LEFTUP;
								break;
								case ACV_MOUSE_RIGHT:
									down = MOUSEEVENTF_RIGHTDOWN;
									up = MOUSEEVENTF_RIGHTUP;
								break;
								default:
									continue;
							}
    
							INPUT[] buffer = Arrays.InitializeWithDefaultInstances<INPUT>(2);
    
							buffer.type = INPUT_MOUSE;
							buffer.mi.mouseData = 0;
							buffer.mi.dwFlags = down;
							buffer.mi.time = 0;
							buffer.mi.dwExtraInfo = 0;
    
							(buffer + 1).type = INPUT_MOUSE;
							(buffer + 1).mi.mouseData = 0;
							(buffer + 1).mi.dwFlags = up;
							(buffer + 1).mi.time = 0;
							(buffer + 1).mi.dwExtraInfo = 0;
    
							SendInput(2, buffer, sizeof(INPUT));
    
						}
    
						if (actionSets[i][AS_EXECUTE][i2][AS_MOUSE_HOLD] != 0)
						{
							int down;
							switch (actionSets[i][AS_EXECUTE][i2][AS_MOUSE_HOLD])
							{
								case ACV_MOUSE_LEFT:
									down = MOUSEEVENTF_LEFTDOWN;
									break;
								case ACV_MOUSE_RIGHT:
									down = MOUSEEVENTF_RIGHTDOWN;
									break;
								default:
									continue;
							}
							INPUT input = new INPUT();
							input.type = INPUT_MOUSE;
							input.mi.mouseData = 0;
							input.mi.dwFlags = down;
							input.mi.time = 0;
							input.mi.dwExtraInfo = 0;
							SendInput(1, input, sizeof(INPUT));
						}
    
					}
					}
    
					if (actionSets[i][AS_EXECUTE][i2][AS_CONFIG_LOAD] != 0)
					{
						loadConfig(configFiles[actionSets[i][AS_EXECUTE][i2][AS_CONFIG_LOAD]]);
					}
    
					if (actionSets[i][AS_EXECUTE][i2][AS_WINDOW_SHOW] != 0)
					{
						System.IntPtr activeWindow = GetForegroundWindow();
						ShowWindow(activeWindow, actionSets[i][AS_EXECUTE][i2][AS_WINDOW_SHOW]);
					}
    
				}
    
				actionSetsStatus[i] = true;
    
				for (uint cj = 0; cj < curJoints.Count; cj++)
				{
					g_JointColorPower[curJoints[cj]] = 1;
				}
    
			}
			else if (actionSetsStatus[i])
			{
    
				actionSetsStatus[i] = false;
    
				//m_Log << "off : " << (int)i << "\n";
				for (int cb = 0; cb < curBodyParts.Count; cb++)
				{
					//m_Log << "\t" << curBodyParts[cb] << "\n";
				}
				//m_Log << "\t";
				//m_Log << "\td:" << distance;
				//m_Log << "\taX:" << angleX;
				//m_Log << "\taY:" << angleY;
				//m_Log << "\n";
    
				for (uint i2 = 0; i2 < actionSets[i][AS_EXECUTE].Count; i2++)
				{
    
					if (actionSets[i][AS_EXECUTE][i2][AS_KEY_HOLD] != 0)
					{
						//m_Log << "keyRelease : " << actionSets[i][AS_EXECUTE][i2][AS_KEY_HOLD] << "\n";
						_beginthread(keyRelease, 0, (object)actionSets[i][AS_EXECUTE][i2][AS_KEY_HOLD]);
					}
    
					if (actionSets[i][AS_EXECUTE][i2][AS_MOUSE_HOLD] != 0)
					{
						int up;
						switch (actionSets[i][AS_EXECUTE][i2][AS_MOUSE_HOLD])
						{
							case ACV_MOUSE_LEFT:
								up = MOUSEEVENTF_LEFTUP;
								break;
							case ACV_MOUSE_RIGHT:
								up = MOUSEEVENTF_RIGHTUP;
								break;
							default:
								continue;
						}
						INPUT input = new INPUT();
						input.type = INPUT_MOUSE;
						input.mi.mouseData = 0;
						input.mi.dwFlags = up;
						input.mi.time = 0;
						input.mi.dwExtraInfo = 0;
						SendInput(1, input, sizeof(INPUT));
					}
					if (actionSets[i][AS_EXECUTE][i2][AS_MOUSE_TRACK] != 0 || actionSets[i][AS_EXECUTE][i2][AS_WINDOW_HOLD] != 0)
					{
						actionSets[i][AS_EXECUTE][i2].Remove(AS_JOINT_PREV_X);
						actionSets[i][AS_EXECUTE][i2].Remove(AS_JOINT_PREV_Y);
					}
				}
    
			}
    
		}
    
		return;
    
	}
}