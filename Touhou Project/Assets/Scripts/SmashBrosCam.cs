using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmashBrosCam : MonoBehaviour
{

   public List<Transform> mTargets;

   public Vector3 mOffset;

   public float xMin;
   public float xMax;
   public float yMin;
   public float yMax;

   public float lerpSpeed;

   int mPlayerOneScore = 0;
   int mPlayerTwoScore = 0;

   Text mPlayerScoreText;
   Text mCountdownText;

   Text PauseText;

   public float mMinSize = 5;
   public float mMaxSize = 8;
   public float mResizeSpeed = .1f;
   public float mMaxDistance = 5f;

   void Start()
   {
      mPlayerScoreText = GameObject.Find("PlayerScoreText").GetComponent<Text>();
      mPlayerScoreText.gameObject.SetActive(false);
      mCountdownText = GameObject.Find("Countdown").GetComponent<Text>();
      StartCoroutine(Countdown());
      PauseText = GameObject.Find("Pause").GetComponent<Text>();
      PauseText.gameObject.SetActive(false);

   }
   IEnumerator Countdown()
   {
      mCountdownText.text = "3";
      for (int i = 0; i < mTargets.Count; i++)
      {
         mTargets[i].GetComponent<PlayerController>().mCanInput = false;
      }

      yield return new WaitForSeconds(1.0f);
      mCountdownText.text = "2";

      yield return new WaitForSeconds(1.0f);
      mCountdownText.text = "1";

      yield return new WaitForSeconds(1.0f);
      mCountdownText.text = "GO!";
      for (int i = 0; i < mTargets.Count; i++)
      {
         mTargets[i].GetComponent<PlayerController>().mCanInput = true;
      }
      yield return new WaitForSeconds(0.5f);
      mCountdownText.gameObject.SetActive(false);
   }


   void LateUpdate()
   {
      Vector3 centerPoint = GetCenterPoint();

      centerPoint += mOffset;
      centerPoint.x = Mathf.Clamp(centerPoint.x, xMin, xMax);
      centerPoint.y = Mathf.Clamp(centerPoint.y, yMin, yMax);

      transform.position = Vector3.Lerp(transform.position, centerPoint, lerpSpeed);
      if ((mTargets[1].position - mTargets[0].position).magnitude >= mMaxDistance)
      {
         GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, mMaxSize, mResizeSpeed);
      }
      else if ((mTargets[1].position - mTargets[0].position).magnitude < mMaxDistance)
      {
         GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, mMinSize, mResizeSpeed);
      }
      //SpawnPlayer();
      if (Time.timeScale == 0.0f)
      {
         PauseText.gameObject.SetActive(true);
      }
      else if (Time.timeScale != 0.0f || !PauseText.gameObject.active)
      {
         PauseText.gameObject.SetActive(false);
      }

   }

   Vector3 GetCenterPoint()
   {
      var bounds = new Bounds(mTargets[0].position, Vector3.zero);
      for (int i = 0; i < mTargets.Count; i++)
      {
         bounds.Encapsulate(mTargets[i].position);
      }

      return bounds.center;
   }

   void SpawnPlayer()
   {
      for (int i = 0; i < mTargets.Count; i++)
      {
         if (!mTargets[i].GetComponent<Renderer>().isVisible)
         {
            mTargets[i].position = Vector3.zero;
            mTargets[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            mTargets[i].GetComponent<PlayerMotor>().mDamageMultiplier = 0;
         }
      }
   }

   private void OnTriggerExit2D(Collider2D collision)
   {
      if (collision.gameObject.name == "Player1")
      {
         mPlayerTwoScore++;
         ResetPlayers();
      }
      else if (collision.gameObject.name == "Player2")
      {
         mPlayerOneScore++;
         ResetPlayers();
      }

   }

   void ResetPlayers()
   {
      GameObject.Find("Player1").GetComponent<ReimuInput>().mDownSpecialUsed = false;
      GameObject.Find("Player1").GetComponent<ReimuInput>().mUpSpecialUsed = false;
      GameObject.Find("Player1").GetComponent<ReimuInput>().mSideSpecialUsed = false;
      GameObject.Find("Player1").GetComponent<ReimuInput>().mNeutralSpecialUsed = false;

      GameObject.Find("Player2").GetComponent<MarisaInput>().mDownSpecialUsed = false;
      GameObject.Find("Player2").GetComponent<MarisaInput>().mUpSpecialUsed = false;
      GameObject.Find("Player2").GetComponent<MarisaInput>().mSideSpecialUsed = false;
      GameObject.Find("Player2").GetComponent<MarisaInput>().mNeutralSpecialUsed = false;

      if(GameObject.Find("LandMine(Clone)"))
         Destroy(GameObject.Find("LandMine(Clone)"));
      if (GameObject.Find("Explosion(Clone)"))
         Destroy(GameObject.Find("Explosion(Clone)"));

      for (int i = 0; i < mTargets.Count; i++)
      {

         switch (i)
         {
            case 0:
               mTargets[i].position = new Vector2(i - 1f, 0f);
               break;
            case 1:
               mTargets[i].position = new Vector2(i + 1f, 0f);
               break;
         }
         mTargets[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
         mTargets[i].GetComponent<PlayerMotor>().mDamageMultiplier = 0;

      }
      StartCoroutine(ScoreShow());

   }

   IEnumerator ScoreShow()
   {
      for (int i = 0; i < mTargets.Count; i++)
      {
         mTargets[i].GetComponent<PlayerController>().mCanInput = false;
      }
      mPlayerScoreText.gameObject.SetActive(true);
      yield return new WaitForSeconds(1f);
      mPlayerScoreText.text = mPlayerOneScore + " - " + mPlayerTwoScore;
      yield return new WaitForSeconds(2f);
      for (int i = 0; i < mTargets.Count; i++)
      {
         mTargets[i].GetComponent<PlayerController>().mCanInput = true;
      }
      mPlayerScoreText.gameObject.SetActive(false);
   }
}
