using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonScript : MonoBehaviour
{
    //バトルマネージャー同期
    [SerializeField]private BattleManager battle_Man = default;
    //バトル開始
    public void BattleStart(){
        battle_Man.start = true;
        SwitchPanel();
        battle_Man.GiveUp.SetActive(true);
    }
    //表示切り替え
    public void SwitchPanel(){
        battle_Man.SwitchPanel();
    }
    //攻撃力強化
    public void PowerUp(){
        battle_Man.StatusUp(1);
    }
    //防御力強化
    public void DiffenceUp(){
        battle_Man.StatusUp(2);
    }
    //回復力強化
    public void HealUp(){
        battle_Man.StatusUp(3);
    }
    //回復
    public void Heal(){
        battle_Man.StatusUp(4);
    }
    //ステータス初期化
    public void Reset(){
        battle_Man.Reset();
    }
    public void GiveUp(){
        battle_Man.start = false;
        battle_Man.GameOverPanel.SetActive(true);
    }
    //バトルシーンへ行く
    public void GoBattle(){
        SceneManager.LoadScene("BattleScene");
    }
    //メインシーンに戻る
    public void GoHome(){
        SceneManager.LoadScene("MainScene");
    }
    //ゲーム終了
    public void Exit(){
        #if UNITY_EDITOR    //エディタに依存
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
