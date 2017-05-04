﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager> 
{
	[System.Serializable]
	public class InputState
	{
		public float 		HoldTime = 0.5f;
		public float 		HoldMoveLimit = 10.0f;
		public Vector2 		EdgeDistPan = new Vector2(0.15f, 0.15f);
		public float 		ThresholdStartEdgePan = 10.0f;

		public float 		InputDownTime {get;set;}
		public Vector3 		InputDownPos {get;set;}
	}

	public InputState m_inputState = new InputState();
	public SmoothMouseLook m_mouseLook;
	public WallDragger m_wallDragger;

	void Start () 
	{
		MusicWall.Instance.OnWallDataUpdated += UpdateProperties;
	}

	public void UpdateProperties(MusicWallData wallData)
	{
		m_wallDragger.Reset(0, -wallData.GetTotalHeight(), wallData.CompositionData.NumCols);
	}

	public bool IsUIBlockingGameInput()
	{
		return MusicWallUI.Instance.IsBlockingGameInput();;
	}

	void Update()
	{
		UpdateGestures();
		UpdateKeyCommands();
		UpdatingObject.Check();
		UpdateDragPanning();
	}

	void UpdateGestures()
	{
		if (Input.GetMouseButtonDown(0))
		{	
			SequencerButtonInputHander.s_sequencerButtonDrag = null;
			m_inputState.InputDownTime = Time.time;
			m_inputState.InputDownPos = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0))
		{
			m_inputState.InputDownTime = float.MaxValue;
		}

		InputConsumerBase.UpdateConsumers(m_inputState);
	}

	void UpdateDragPanning()
	{
		bool isButtonDragActive = SequencerButtonInputHander.s_sequencerButtonDrag != null;
		var inputDelta = Input.mousePosition - m_inputState.InputDownPos;

		if (isButtonDragActive 
			&& EdgePanningUtil.DragTreshReached((inputDelta).magnitude, m_inputState.ThresholdStartEdgePan))
		{
			var pan = EdgePanningUtil.EdgeDragPanAmount(Input.mousePosition, 
				m_inputState.EdgeDistPan.x,
				m_inputState.EdgeDistPan.y);	
			if (pan.sqrMagnitude > 0)
				m_wallDragger.PerformPan(pan);
		}
	}

	void UpdateKeyCommands()
	{
		if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyUp(KeyCode.Z))
		{
			MusicWall.Instance.WallProperties.CompositionData.CommandManager.Undo();
		}
	}

	void LateUpdate()
	{
		LateUpdateGestures();
	}

	void LateUpdateGestures()
	{
		if (Input.GetMouseButtonUp(0))
		{
			// end drag, even if released not over a button
			SequencerButtonInputHander.s_sequencerButtonDrag = null;
		}
	}
}
