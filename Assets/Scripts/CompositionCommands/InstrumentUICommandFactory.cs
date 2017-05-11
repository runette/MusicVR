﻿using System.Collections;
using UnityEngine;

namespace CompositionCommands
{
	/// <summary>
	/// Links each instrument command type to its concrete command. 
	/// Command type to be set via dropdown menu on InstrumentUIData scriptable object. 
	/// </summary>
	public class InstrumentUICommandFactory
	{
		public static Command Create(InstrumentUIData.E_InstrumentCommand type, CompositionData.InstrumentData data)
		{
			switch (type)
			{
			case InstrumentUIData.E_InstrumentCommand.toggleScale:
				return new ToggleScaleCommand(data);
			case InstrumentUIData.E_InstrumentCommand.toggleInstrument:
				return new ToggleInstrumentCommand(data);
			default:
				Debug.LogError("Unhandled command type:" + type.ToString());
				return null;
			}
		}
	}
}