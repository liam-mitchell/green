﻿using UnityEngine;
using System.Collections;

public enum MessageTypes : short {
	REQ_PLAYER = 51,
	PLAYER,
	CREATE_PLAYER,
	PLAYER_CREATED,
	PLAYER_NOT_CREATED,
	REQ_PLAYER_DATA,
	PLAYER_DATA,
	SAVE_PLAYER,
	PLAYER_SAVED,
	PLAYER_NOT_SAVED,
	CHANGE_SCENE
}