﻿using System; using System.Collections; using System.Collections.Generic; using UnityEngine;  public class Calibrator : MonoBehaviour {  	public static Vector3 gyroNoise; 	public static Vector3 accelNoise; 	public static Vector3 magnetNoise;  	private Vector3[] gyroSamples; 	private Vector3[] accelSamples; 	private Vector3[] magnetSamples; 	private UDPClient client; 	private int counter; 	// Use this for initialization 	void Start () {  		gyroNoise = Vector3.zero; 		accelNoise = Vector3.zero; 		magnetNoise = Vector3.zero;  		gyroSamples = new Vector3[1000]; 		accelSamples = new Vector3[1000]; 		magnetSamples = new Vector3[1000]; 		counter = 0; 		client = FindObjectOfType<UDPClient> (); 		if (!client) { 			Debug.LogError ("Client not found in " + name + ", impossible to calibrate"); 		} 	}  	public void Init(){  		client.StartTransmition (); 		CalibrateGyroscope (); //		CalibrateAccelerometer (); //		CalibrateMagnetometer (); 	}  	private void CalibrateGyroscope(){ 		 		StartCoroutine(TakeSamples(2)); 	}  	private void CalibrateAccelerometer(){  		for (int i=0;i<1000;i++) { 			accelSamples[i] = client.GetAccelerometer (); 		} 		print ("Accelerometer samples taken"); 		print (accelSamples[999]); 	}  	private void CalibrateMagnetometer(){  		for (int i=0;i<1000;i++) { 			magnetSamples[i] = client.GetMagnetometer (); 		} 		print ("Magnetometer samples taken"); 		print (magnetSamples[999]); 	}  	public void Finish(){  		client.StopTransmition (); 	}  	IEnumerator TakeSamples(int number){  		if(number == 1){ 			for (int i = 0; i < 1000; i++) { 				gyroSamples [i] = Transmitter.gyroscope; 				yield return new WaitForSeconds (0.01f); 			} 			Vector3 mean = Vector3.zero; 			for(int i=0;i<1000;i++){ 				mean += gyroSamples [i]; 			} 			mean = mean / 1000; 			print ("NOISE OF GYRO: " + mean); 		} 		else if(number == 2){ 			for (int i = 0; i < 1000; i++) { 				accelSamples [i] = Transmitter.accelerometer; 				yield return new WaitForSeconds (0.01f); 			} 			Vector3 mean = Vector3.zero; 			for(int i=0;i<1000;i++){ 				mean += (accelSamples [i] - new Vector3(0f,0f,9.81f)); 			} 			mean = mean / 1000; 			print ("NOISE OF ACCEL: " + mean); 		} 		else if(number == 3){ 			for (int i = 0; i < 1000; i++) { 				magnetSamples [i] = Transmitter.magnetometer; 				yield return new WaitForSeconds (0.01f); 			} 			Debug.Log ("Finished"); 		} 	} }