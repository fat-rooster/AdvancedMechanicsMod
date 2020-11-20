
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Screens.Helpers;
using Sandbox.ModAPI;
//using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;




namespace MechanicsMod
{
	//[MyEntityComponentDescriptor(typeof(MyObjectBuilder_MotorAdvancedStator), false )]
	public class RotorTools : MyGameLogicComponent
	{
		public float rpmStep = 1f;

		

		public override void OnAddedToContainer()
        {
			if(!controlsCreated)
			{
				createTerminalControls();
			}
        }

		public static void stepUp(IMyTerminalBlock block)
		{
			var motor = block as IMyMotorStator;
			var rotor = block.GameLogic.GetAs<RotorTools>();
			if (motor != null && rotor != null)
			{
				motor.TargetVelocityRPM += rotor.rpmStep;
			}
		}
		public static void stepDown(IMyTerminalBlock block)
		{
			var motor = block as IMyMotorStator;
			var rotor = block.GameLogic.GetAs<RotorTools>();
			if (motor != null && rotor != null)
			{
				motor.TargetVelocityRPM -= rotor.rpmStep;
			}
		}
		public static void setUp(IMyTerminalBlock block)
		{
			var motor = block as IMyMotorStator;
			var rotor = block.GameLogic.GetAs<RotorTools>();
			if (motor != null && rotor != null)
			{
				motor.TargetVelocityRPM = rotor.rpmStep;
			}
		}
		public static void setDown(IMyTerminalBlock block)
		{
			var motor = block as IMyMotorStator;
			var rotor = block.GameLogic.GetAs<RotorTools>();
			if (motor != null && rotor != null)
			{
				motor.TargetVelocityRPM = -rotor.rpmStep;
			}
		}
		/*public static void goTo(IMyTerminalBlock block,ListReader<TerminalActionParamater> paramater )
        {

        }*/
		public static void createTerminalControls()
		{
			if (!controlsCreated)
			{
				var con = MyAPIGateway.TerminalControls;
				controlsCreated = true;
				var stepUpAction = con.CreateAction<IMyMotorAdvancedStator>("stepUp");
				stepUpAction.Action = stepUp;
				stepUpAction.Writer = (b, v) => v.Append(string.Format("+{0:N1}", b.GameLogic.GetAs<RotorTools>().rpmStep));
				stepUpAction.Name = new StringBuilder("Rpm Step Up");
				stepUpAction.Icon = MechanicsMod.MyTerminalActionIcons.INCREASE;

				var stepDownAction = con.CreateAction<IMyMotorAdvancedStator>("stepDown");
				stepDownAction.Action = stepDown;
				stepDownAction.Writer = (b, v) => v.Append(string.Format("-{0:N1}", b.GameLogic.GetAs<RotorTools>().rpmStep));
				stepDownAction.Name = new StringBuilder("Rpm Step Down");
				stepDownAction.Icon = MechanicsMod.MyTerminalActionIcons.DECREASE;

				var setUpAction = con.CreateAction<IMyMotorAdvancedStator>("SetUp");
				setUpAction.Action = setUp;
				setUpAction.Writer = (b, v) => v.Append(string.Format("-{0:N1}", b.GameLogic.GetAs<RotorTools>().rpmStep));
				setUpAction.Name = new StringBuilder("Rpm Set Up");
				setUpAction.Icon = MechanicsMod.MyTerminalActionIcons.INCREASE;

				var setDownAction = con.CreateAction<IMyMotorAdvancedStator>("SetDown");
				setDownAction.Action = setDown;
				setDownAction.Writer = (b, v) => v.Append(string.Format("-{0:N1}", b.GameLogic.GetAs<RotorTools>().rpmStep));
				setDownAction.Name = new StringBuilder("Rpm Set Down");
				setDownAction.Icon = MechanicsMod.MyTerminalActionIcons.DECREASE;

				con.AddAction<IMyMotorAdvancedStator>(stepUpAction);
				con.AddAction<IMyMotorAdvancedStator>(stepDownAction);
				con.AddAction<IMyMotorAdvancedStator>(setUpAction);
				con.AddAction<IMyMotorAdvancedStator>(setDownAction);

				var newLabel = con.CreateControl<IMyTerminalControlSeparator, IMyMotorAdvancedStator>("ExtraControls_Separator");
				var newTitle = con.CreateControl<IMyTerminalControlLabel, IMyMotorAdvancedStator>("ExtraControlsTitle");
				var rpmAdjuster = con.CreateControl<IMyTerminalControlSlider, IMyMotorAdvancedStator>("rpmAdjuster");

				newTitle.Label = MyStringId.GetOrCompute("AdvancedControls");
				rpmAdjuster.SetLimits(0f, 20f);
				rpmAdjuster.Getter = (b) => b.GameLogic.GetAs<RotorTools>().rpmStep;
				rpmAdjuster.Setter = (b, x) => b.GameLogic.GetAs<RotorTools>().rpmStep = x;
				rpmAdjuster.Title = MyStringId.GetOrCompute("Rpm step size");
				rpmAdjuster.Writer = (b, v) => v.Append(string.Format("{0:N1} rpm/action", b.GameLogic.GetAs<RotorTools>().rpmStep));


				con.AddControl<IMyMotorAdvancedStator>(newLabel);
				con.AddControl<IMyMotorAdvancedStator>(newTitle);
				con.AddControl<IMyMotorAdvancedStator>(rpmAdjuster);

				
			}
		}
		static bool controlsCreated = false;

	}
}
