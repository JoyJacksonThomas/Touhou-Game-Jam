using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLooper : MonoBehaviour {

   public AudioClip mIntro;
   public AudioClip mLoop;

   public AudioSource mAudioSource;

   bool mSwitchedAudioClips = false;

   void Awake()
   {
      mAudioSource.clip = mIntro;
      mAudioSource.Play(0);
   }
   
   void Update ()
   {
		if(!mAudioSource.isPlaying && !mSwitchedAudioClips)
      {
         mAudioSource.clip = mLoop;
         mAudioSource.Play(0);
         mSwitchedAudioClips = true;
         mAudioSource.loop = true;
      }
	}
}
