using UnityEngine;
using System.Collections.Generic;

public class ShipInputHandlers {
	public delegate void AxisHandler(float value);
	public delegate void ButtonHandler(bool value);

	private Dictionary<string, List<AxisHandler>> axisHandlers;
	private Dictionary<string, List<ButtonHandler>> buttonHandlers;

	private PlayerInput input;

	public ShipInputHandlers(PlayerInput i) {
		axisHandlers = new Dictionary<string, List<AxisHandler>>();
		buttonHandlers = new Dictionary<string, List<ButtonHandler>>();
		input = i;
	}

	public void Update () {
		var axisValues = new Dictionary<string, float>() {
			{"Horizontal", input.state.horizontal},
			{"Vertical", input.state.vertical},
			{"HorizontalLook", input.state.horizontalLook},
			{"VerticalLook", input.state.verticalLook},
		};

		var buttonValues = new Dictionary<string, bool>() {
			{"Fire", input.state.fire},
			{"Locking", input.state.locking},
			{"CraftingMode", input.state.craftingMode}
		};

		foreach (var val in axisValues) {
			if (val.Value != 0.0f) {
				CallHandlers(val.Key, val.Value);
			}
		}

		foreach (var val in buttonValues) {
			if (val.Value) {
				CallHandlers(val.Key, val.Value);
			}
		}
	}

	public void RegisterHandler(string axis, AxisHandler handler) {
		if (axisHandlers[axis] == null) {
			axisHandlers[axis] = new List<AxisHandler>();
		}

		axisHandlers[axis].Add(handler);
	}

	public void RegisterHandler(string axis, ButtonHandler handler) {
		if (buttonHandlers[axis] == null) {
			buttonHandlers[axis] = new List<ButtonHandler>();
		}

		buttonHandlers[axis].Add (handler);
	}

	private void CallHandlers(string axis, float value) {
		foreach (var handler in axisHandlers[axis]) {
			handler(value);
		}
	}

	private void CallHandlers(string axis, bool value) {
		foreach (var handler in buttonHandlers[axis]) {
			handler(value);
		}
	}
}
