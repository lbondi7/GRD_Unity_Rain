using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

[CustomEditor(typeof(WettableSurface))]
public class W_SurfaceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WettableSurface myObject = (WettableSurface)target;
        
        GUILayout.Label(" -----------------------Wetness details-------------------------------");
        EditorGUILayout.LabelField("State: ", myObject.CurrentWetState.ToString());
        EditorGUILayout.LabelField("Time spent in rain", myObject.wetness.ToString());
        EditorGUILayout.LabelField("Wetness Level", myObject.currentWetnessLevel.ToString());
        
        GUILayout.Space(25);
        GUILayout.Label("Object wetness will change once these time thresholds are met");

        SerializedProperty thresholdList = serializedObject.FindProperty("wetnessTimeThresholds");
        EditorGUILayout.PropertyField(thresholdList, true);
        SerializedProperty resultList = serializedObject.FindProperty("thresholdReflectiveness");
        resultList.arraySize = thresholdList.arraySize;
        SerializedProperty visThresholdList = serializedObject.FindProperty("thresholdVisibility");
        visThresholdList.arraySize = thresholdList.arraySize;
        
        
        myObject.reflectionsUsed = EditorGUILayout.Toggle("Change Reflectiveness?", myObject.reflectionsUsed);
        serializedObject.ApplyModifiedProperties();
        if (myObject.reflectionsUsed)
            ShowReflectionField(resultList, thresholdList);
        else
            ClearReflectionField(resultList);
        
        GUILayout.Space(40);

        myObject.fadingUsed = EditorGUILayout.Toggle("Change Transparency?", myObject.fadingUsed);
        if (myObject.fadingUsed)
            ShowFadingField(visThresholdList);
        serializedObject.ApplyModifiedProperties();

        
    }

    private void ClearReflectionField(SerializedProperty resultList)
    {
        var arraySize = resultList.arraySize + 2;
        for (int i = 0; i < arraySize; i++)
        {
            if (i < 2)
            {
                resultList.Next(true);
                continue;
            }

            resultList.Next(true);
            resultList.floatValue = 0;
        }

        resultList.Reset();
    }

    void ShowReflectionField(SerializedProperty reflectList, SerializedProperty thresholdList)
    {
        GUILayout.Label("Reflectiveness increases from 0-1000");
        var arraySize = reflectList.arraySize + 2;
        for (int i = 0; i < arraySize; i++)
        {
            if (i < 2)
            {
                reflectList.Next(true);
                thresholdList.Next(true);
                continue;
            }
            reflectList.Next(true);
            thresholdList.Next(true);
            
            var labelTitle = "Reflectiveness value " + thresholdList.intValue.ToString() +"s:";
            float currentIndex = reflectList.floatValue;
            reflectList.floatValue = EditorGUILayout.Slider(labelTitle,reflectList.floatValue, 0f, 1000f);
            
            
            serializedObject.ApplyModifiedProperties();
        }
        reflectList.Reset();
        thresholdList.Reset();
    }

    void ShowFadingField(SerializedProperty visList)
    {
        SerializedProperty fadeMaterial = serializedObject.FindProperty("fMaterial");
        GUILayout.Label("Drag in fade-type material the object will switch to.");
        EditorGUILayout.PropertyField(fadeMaterial);
        serializedObject.ApplyModifiedProperties();
        SerializedProperty fadeStateCondition = serializedObject.FindProperty("fadingTrigger");
        
        EditorGUILayout.LabelField("Choose the state required for object to fade");
        EditorGUILayout.PropertyField(fadeStateCondition);

        switch (fadeStateCondition.enumValueIndex)
        {
            case (0):
                SerializedProperty outOfRainVisibility = serializedObject.FindProperty("outOfRainFadeStrength");
                outOfRainVisibility.floatValue = EditorGUILayout.Slider(outOfRainVisibility.floatValue, 100f, 0f);
                break;
            case (1):
                var arraySize = visList.arraySize + 2;
                for (int i = 0; i < arraySize; i++)
                {
                    if (i < 2)
                    {
                        visList.Next(true);
                        continue;
                    }
                    visList.Next(true);
            
                    var labelTitle = "Fade value " +(i-1);
                    float currentIndex = visList.floatValue;
                    visList.floatValue = EditorGUILayout.Slider(labelTitle,visList.floatValue, 100f, 0f);
                    
                }
                visList.Reset();
                break;
            case(2):
                SerializedProperty fullyWetVisibility = serializedObject.FindProperty("submergedFadeStrength");
                fullyWetVisibility.floatValue = EditorGUILayout.Slider(fullyWetVisibility.floatValue, 100f, 0f);
                break;
        }

        SerializedProperty fadeMultiplier = serializedObject.FindProperty("fadingSpeedMultiplier");
        EditorGUILayout.LabelField("Multiply the speed of the fade. Leave at 1 for normal speed.");
        EditorGUILayout.PropertyField(fadeMultiplier);
        serializedObject.ApplyModifiedProperties();
    }
    
    
}
