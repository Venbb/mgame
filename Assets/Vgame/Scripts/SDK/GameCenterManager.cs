using UnityEngine;
using Vgame;
using UnityEngine;

using UnityEngine.SocialPlatforms;

using UnityEngine.SocialPlatforms.GameCenter;

using System.Collections;

/// <summary>
/// 作者:niko
/// 创建时间:2015/08/31 11:51:22
/// 描述:
/// </summary>
public class GameCenterManager : MonoBehaviour
{
	// Use this for initialization

	void Start ()
	{
		Social.localUser.Authenticate (HandleAuthenticated);
	}

	#region 事件回调

	private void HandleAuthenticated (bool success)
	{

		Debug.Log ("*** HandleAuthenticated: success = " + success);

		if (success)
		{
			Debug.Log ("Social.localUser.id:" + Social.localUser.id);
			Debug.Log ("Social.Active:" + Social.Active);
			Debug.Log ("Social.localUser.authenticated:" + Social.localUser.authenticated);
			Debug.Log ("Social.localUser.friends:" + Social.localUser.friends);
			Debug.Log ("Social.localUser.image:" + Social.localUser.image);
			Debug.Log ("Social.localUser.isFriend:" + Social.localUser.isFriend);
			Debug.Log ("Social.localUser.state:" + Social.localUser.state);
			Debug.Log ("Social.localUser.underage:" + Social.localUser.underage);
			Debug.Log ("Social.localUser.userName:" + Social.localUser.userName);

			Social.LoadUsers (new string[]{ "" + Social.localUser.id }, HandleUsersLoaded);

			Social.localUser.LoadFriends (HandleFriendsLoaded);

			Social.LoadAchievements (HandleAchievementsLoaded);

			Social.LoadAchievementDescriptions (HandleAchievementDescriptionsLoaded);

			Social.LoadScores ("com.7k7k.sheep.10001", HandleScoresLoaded);

			ReportScore ("com.7k7k.sheep.10001", 254);
			ShowLeaderboard ();

		}

	}


	void HandleUsersLoaded (IUserProfile[] users)
	{
		Debug.Log ("*** HandleUsersLoaded");
		foreach (IUserProfile user in users)
		{
			Debug.Log ("*   user = " + user.userName + ";id:" + user.id);
		}
	}

	private void HandleFriendsLoaded (bool success)
	{

		Debug.Log ("*** HandleFriendsLoaded: success = " + success);

		foreach (IUserProfile friend in Social.localUser.friends)
		{

			Debug.Log ("*   friend = " + friend.ToString ());

		}

	}

	private void HandleAchievementsLoaded (IAchievement[] achievements)
	{

		Debug.Log ("*** HandleAchievementsLoaded");

		foreach (IAchievement achievement in achievements)
		{

			Debug.Log ("*   achievement = " + achievement.ToString ());

		}

	}

	private void HandleAchievementDescriptionsLoaded (IAchievementDescription[] achievementDescriptions)
	{

		Debug.Log ("*** HandleAchievementDescriptionsLoaded");

		foreach (IAchievementDescription achievementDescription in achievementDescriptions)
		{

			Debug.Log ("*   achievementDescription = " + achievementDescription.ToString ());

		}

	}

	void HandleScoresLoaded (IScore[] scores)
	{
		Debug.Log ("*** HandleScoresLoaded");
		foreach (IScore score in scores)
		{
			Debug.Log ("*    score.date = " + score.date + "score.formattedValue = " + score.formattedValue + "score.leaderboardID = " + score.leaderboardID + "score.rank = " + score.rank + "score.userID = " + score.userID + "score.value = " + score.value);
		}
	}

	#endregion



	#region 成就系统

	/// <summary>

	/// 成就进度

	/// </summary>

	/// <param name="achievementId">Achievement identifier.</param>

	/// <param name="progress">Progress.</param>

	public void ReportProgress (string achievementId, double progress)
	{

		if (Social.localUser.authenticated)
		{

			Social.ReportProgress (achievementId, progress, HandleProgressReported);

		}

	}

	private void HandleProgressReported (bool success)
	{

		Debug.Log ("*** 成就进度上传: success = " + success);

	}



	/// <summary>

	/// 显示成就

	/// </summary>

	public void ShowAchievements ()
	{

		if (Social.localUser.authenticated)
		{

			Social.ShowAchievementsUI ();

		}

	}

	#endregion



	#region 排名系统

	/// <summary>

	/// 上传积分

	/// </summary>

	/// <param name="leaderboardId">Leaderboard identifier.</param>

	/// <param name="score">Score.</param>

	public void ReportScore (string leaderboardId, long score)
	{

		if (Social.localUser.authenticated)
		{

			Social.ReportScore (score, leaderboardId, HandleScoreReported);

		}

	}

	public void HandleScoreReported (bool success)
	{

		Debug.Log ("*** 排名积分上传: success = " + success);

	}



	/// <summary>

	/// 显示排名

	/// </summary>

	public void ShowLeaderboard ()
	{

		if (Social.localUser.authenticated)
		{

			Social.ShowLeaderboardUI ();

		}

	}

	#endregion

}