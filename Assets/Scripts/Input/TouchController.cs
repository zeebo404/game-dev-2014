﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(InputHandler),typeof(Grid))]
public class TouchController : MonoBehaviour {

	private InputHandler handler;
	private Grid grid;

	private Vector3 touchPosition;
	private Vector3 worldTouchPosition;

	public float CircleRadius = 2f;

	// Use this for initialization
	void Start () {
		grid = GetComponent<Grid>();
		handler = GetComponent<InputHandler>();
	}
	
	// Update is called once per frame
	void Update () {
		handler.handleInput();

		foreach(InputEvent e in handler.Events) {
			if (e.phase == TouchPhase.Began) {
				touchPosition = e.position;
				worldTouchPosition = Utilities.ScreenToWorld(touchPosition);
				ShowTowers();
			}
			if (e.phase == TouchPhase.Ended) {
				bool placed = grid.PlaceTower(TowerSelectionMenu.Instance.towers[GetIndex(e.position)], touchPosition);

				HideTowers();
			}
		}
	}

	void ShowTowers() {
		for (int i = 0; i < 8; i++) {
			TowerSelectionMenu.Instance.towerInstances[i].transform.position = Circularize(i);
			TowerSelectionMenu.Instance.towerInstances[i].SetActive(true);
		}
	}

	void HideTowers() {
		for (int i = 0; i < 8; i++) {
			TowerSelectionMenu.Instance.towerInstances[i].SetActive(false);
		}
	}

	int GetIndex(Vector3 release) {

		float rawAngle = Mathf.Atan2(release.y - touchPosition.y, release.x - touchPosition.x) - Mathf.PI / 2 - Mathf.PI / 8;
		rawAngle += rawAngle < 0 ? Mathf.PI * 2 : 0;

		return (int)(8f - (rawAngle / (Mathf.PI * 2)) * 8);
	}

	Vector3 Circularize(int index) {
		float angle = Mathf.PI / 2 + index / 8.0f * Mathf.PI * 2;

		return new Vector3(
			Mathf.Cos(angle) * CircleRadius + worldTouchPosition.x,
			Mathf.Sin(angle) * CircleRadius - worldTouchPosition.y,
			0
		);
	}
}