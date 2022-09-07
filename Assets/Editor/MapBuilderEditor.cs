using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WorldGeneration {

  [CustomEditor (typeof(MapGenerator))]
  public class MapBuilderEditor : Editor {


    public override void OnInspectorGUI() {
      MapGenerator mapGenerator = (MapGenerator)target;

      DrawDefaultInspector();

      if (GUILayout.Button("Regenerate") && Application.isPlaying) {
        mapGenerator.GenerateMap();
      }
    }

  }
}