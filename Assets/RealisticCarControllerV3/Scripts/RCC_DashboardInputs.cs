//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2021 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Receiving inputs from active vehicle on your scene, and feeds dashboard needles, texts, images.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/RCC UI Dashboard Inputs")]
public class RCC_DashboardInputs : MonoBehaviour {

	// Getting an Instance of Main Shared RCC Settings.
	#region RCC Settings Instance

	private RCC_Settings RCCSettingsInstance;
	private RCC_Settings RCCSettings {
		get {
			if (RCCSettingsInstance == null) {
				RCCSettingsInstance = RCC_Settings.Instance;
				return RCCSettingsInstance;
			}
			return RCCSettingsInstance;
		}
	}

	#endregion

	public GameObject RPMNeedle;
	public GameObject KMHNeedle;
	public GameObject turboGauge;
	public GameObject turboNeedle;
	public GameObject NOSGauge;
	public GameObject NoSNeedle;
	public GameObject heatGauge;
	public GameObject heatNeedle;
	public GameObject fuelGauge;
	public GameObject fuelNeedle;

	private float RPMNeedleRotation = 0f;
	private float KMHNeedleRotation = 0f;
	private float BoostNeedleRotation = 0f;
	private float NoSNeedleRotation = 0f;
	private float heatNeedleRotation = 0f;
	private float fuelNeedleRotation = 0f;

	internal float RPM;
	internal float KMH;
	internal int direction = 1;
	internal float Gear;
	internal bool changingGear = false;
	internal bool NGear = false;

	internal bool ABS = false;
	internal bool ESP = false;
	internal bool Park = false;
	internal bool Headlights = false;

	internal RCC_CarControllerV3.IndicatorsOn indicators;
	//=== MY CODE START ===
	private CarTag.Player thisPlayer;
    private void Awake() {
		thisPlayer = GetComponentInParent<CarTag.Player>();
    }
	//=== MY CODE END

    void Update(){

		//GetValues();
		GetValuesModifiedByScott();

	}
	void GetValuesModifiedByScott() {
		if (!thisPlayer.RCC_CarController)
			return;

		if (!thisPlayer.RCC_CarController.canControl || thisPlayer.RCC_CarController.externalController)
			return;

		if (NOSGauge) {

			if (thisPlayer.RCC_CarController.useNOS) {

				if (!NOSGauge.activeSelf)
					NOSGauge.SetActive(true);

			}
			else {

				if (NOSGauge.activeSelf)
					NOSGauge.SetActive(false);

			}

		}

		if (turboGauge) {

			if (thisPlayer.RCC_CarController.useTurbo) {

				if (!turboGauge.activeSelf)
					turboGauge.SetActive(true);

			}
			else {

				if (turboGauge.activeSelf)
					turboGauge.SetActive(false);

			}

		}

		if (heatGauge) {

			if (thisPlayer.RCC_CarController.useEngineHeat) {

				if (!heatGauge.activeSelf)
					heatGauge.SetActive(true);

			}
			else {

				if (heatGauge.activeSelf)
					heatGauge.SetActive(false);

			}

		}

		if (fuelGauge) {

			if (thisPlayer.RCC_CarController.useFuelConsumption) {

				if (!fuelGauge.activeSelf)
					fuelGauge.SetActive(true);

			}
			else {

				if (fuelGauge.activeSelf)
					fuelGauge.SetActive(false);

			}

		}

		RPM = thisPlayer.RCC_CarController.engineRPM;
		KMH = thisPlayer.RCC_CarController.speed;
		direction = thisPlayer.RCC_CarController.direction;
		Gear = thisPlayer.RCC_CarController.currentGear;
		changingGear = thisPlayer.RCC_CarController.changingGear;
		NGear = thisPlayer.RCC_CarController.NGear;

		ABS = thisPlayer.RCC_CarController.ABSAct;
		ESP = thisPlayer.RCC_CarController.ESPAct;
		Park = thisPlayer.RCC_CarController.handbrakeInput > .1f ? true : false;
		Headlights = thisPlayer.RCC_CarController.lowBeamHeadLightsOn || thisPlayer.RCC_CarController.highBeamHeadLightsOn;
		indicators = thisPlayer.RCC_CarController.indicatorsOn;

		if (RPMNeedle) {

			RPMNeedleRotation = (thisPlayer.RCC_CarController.engineRPM / 50f);
			RPMNeedle.transform.eulerAngles = new Vector3(RPMNeedle.transform.eulerAngles.x, RPMNeedle.transform.eulerAngles.y, -RPMNeedleRotation);

		}

		if (KMHNeedle) {

			if (RCCSettings.units == RCC_Settings.Units.KMH)
				KMHNeedleRotation = (thisPlayer.RCC_CarController.speed);
			else
				KMHNeedleRotation = (thisPlayer.RCC_CarController.speed * 0.62f);

			KMHNeedle.transform.eulerAngles = new Vector3(KMHNeedle.transform.eulerAngles.x, KMHNeedle.transform.eulerAngles.y, -KMHNeedleRotation);

		}

		if (turboNeedle) {

			BoostNeedleRotation = (thisPlayer.RCC_CarController.turboBoost / 30f) * 270f;
			turboNeedle.transform.eulerAngles = new Vector3(turboNeedle.transform.eulerAngles.x, turboNeedle.transform.eulerAngles.y, -BoostNeedleRotation);

		}

		if (NoSNeedle) {

			NoSNeedleRotation = (thisPlayer.RCC_CarController.NoS / 100f) * 270f;
			NoSNeedle.transform.eulerAngles = new Vector3(NoSNeedle.transform.eulerAngles.x, NoSNeedle.transform.eulerAngles.y, -NoSNeedleRotation);

		}

		if (heatNeedle) {

			heatNeedleRotation = (thisPlayer.RCC_CarController.engineHeat / 110f) * 270f;
			heatNeedle.transform.eulerAngles = new Vector3(heatNeedle.transform.eulerAngles.x, heatNeedle.transform.eulerAngles.y, -heatNeedleRotation);

		}

		if (fuelNeedle) {

			fuelNeedleRotation = (thisPlayer.RCC_CarController.fuelTank / thisPlayer.RCC_CarController.fuelTankCapacity) * 270f;
			fuelNeedle.transform.eulerAngles = new Vector3(fuelNeedle.transform.eulerAngles.x, fuelNeedle.transform.eulerAngles.y, -fuelNeedleRotation);

		}

	}

	void GetValues(){

		if(!RCC_SceneManager.Instance.activePlayerVehicle)
			return;

		if(!RCC_SceneManager.Instance.activePlayerVehicle.canControl || RCC_SceneManager.Instance.activePlayerVehicle.externalController)
			return;

		if(NOSGauge){
			
			if(RCC_SceneManager.Instance.activePlayerVehicle.useNOS){
				
				if(!NOSGauge.activeSelf)
					NOSGauge.SetActive(true);
				
			}else{
				
				if(NOSGauge.activeSelf)
					NOSGauge.SetActive(false);
				
			}

		}

		if(turboGauge){
			
			if(RCC_SceneManager.Instance.activePlayerVehicle.useTurbo){
				
				if(!turboGauge.activeSelf)
					turboGauge.SetActive(true);
				
			}else{
				
				if(turboGauge.activeSelf)
					turboGauge.SetActive(false);
				
			}

		}

		if (heatGauge) {

			if (RCC_SceneManager.Instance.activePlayerVehicle.useEngineHeat) {

				if(!heatGauge.activeSelf)
					heatGauge.SetActive(true);

			}else{

				if(heatGauge.activeSelf)
					heatGauge.SetActive(false);

			}

		}

		if (fuelGauge) {

			if (RCC_SceneManager.Instance.activePlayerVehicle.useFuelConsumption) {

				if(!fuelGauge.activeSelf)
					fuelGauge.SetActive(true);

			}else{

				if(fuelGauge.activeSelf)
					fuelGauge.SetActive(false);

			}

		}
		
		RPM = RCC_SceneManager.Instance.activePlayerVehicle.engineRPM;
		KMH = RCC_SceneManager.Instance.activePlayerVehicle.speed;
		direction = RCC_SceneManager.Instance.activePlayerVehicle.direction;
		Gear = RCC_SceneManager.Instance.activePlayerVehicle.currentGear;
		changingGear = RCC_SceneManager.Instance.activePlayerVehicle.changingGear;
		NGear = RCC_SceneManager.Instance.activePlayerVehicle.NGear;
		
		ABS = RCC_SceneManager.Instance.activePlayerVehicle.ABSAct;
		ESP = RCC_SceneManager.Instance.activePlayerVehicle.ESPAct;
		Park = RCC_SceneManager.Instance.activePlayerVehicle.handbrakeInput > .1f ? true : false;
		Headlights = RCC_SceneManager.Instance.activePlayerVehicle.lowBeamHeadLightsOn || RCC_SceneManager.Instance.activePlayerVehicle.highBeamHeadLightsOn;
		indicators = RCC_SceneManager.Instance.activePlayerVehicle.indicatorsOn;

		if(RPMNeedle){
			
			RPMNeedleRotation = (RCC_SceneManager.Instance.activePlayerVehicle.engineRPM / 50f);
			RPMNeedle.transform.eulerAngles = new Vector3(RPMNeedle.transform.eulerAngles.x ,RPMNeedle.transform.eulerAngles.y, -RPMNeedleRotation);

		}

		if(KMHNeedle){
			
			if(RCCSettings.units == RCC_Settings.Units.KMH)
				KMHNeedleRotation = (RCC_SceneManager.Instance.activePlayerVehicle.speed);
			else
				KMHNeedleRotation = (RCC_SceneManager.Instance.activePlayerVehicle.speed * 0.62f);
			
			KMHNeedle.transform.eulerAngles = new Vector3(KMHNeedle.transform.eulerAngles.x ,KMHNeedle.transform.eulerAngles.y, -KMHNeedleRotation);

		}

		if(turboNeedle){
			
			BoostNeedleRotation = (RCC_SceneManager.Instance.activePlayerVehicle.turboBoost / 30f) * 270f;
			turboNeedle.transform.eulerAngles = new Vector3(turboNeedle.transform.eulerAngles.x ,turboNeedle.transform.eulerAngles.y, -BoostNeedleRotation);

		}

		if(NoSNeedle){
			
			NoSNeedleRotation = (RCC_SceneManager.Instance.activePlayerVehicle.NoS / 100f) * 270f;
			NoSNeedle.transform.eulerAngles = new Vector3(NoSNeedle.transform.eulerAngles.x ,NoSNeedle.transform.eulerAngles.y, -NoSNeedleRotation);

		}

		if(heatNeedle){

			heatNeedleRotation = (RCC_SceneManager.Instance.activePlayerVehicle.engineHeat / 110f) * 270f;
			heatNeedle.transform.eulerAngles = new Vector3(heatNeedle.transform.eulerAngles.x ,heatNeedle.transform.eulerAngles.y, -heatNeedleRotation);

		}

		if(fuelNeedle){

			fuelNeedleRotation = (RCC_SceneManager.Instance.activePlayerVehicle.fuelTank / RCC_SceneManager.Instance.activePlayerVehicle.fuelTankCapacity) * 270f;
			fuelNeedle.transform.eulerAngles = new Vector3(fuelNeedle.transform.eulerAngles.x ,fuelNeedle.transform.eulerAngles.y, -fuelNeedleRotation);

		}
			
	}

}



