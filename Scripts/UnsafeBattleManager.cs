using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

unsafe public class UnsafeBattleManager : MonoBehaviour
{
    //ステータスの段階
    int powerLevel;     //攻撃力
    int diffenceLevel;  //防御力
    int healLevel;      //回復力
    int enemyLevel;     //エネミーのレベル

    //バトル関係
    public bool start;  //バトル開始か？
    float interval;     //バトルの間隔(n秒)

    //Birdクラス
    Bird player = default;  //プレイヤー
    Bird enemy = default;   //エネミー     

    //GameObject
    private GameObject playerObj;
    private GameObject enemyObj;
    [SerializeField]private GameObject PlayerBird = default;    //プレイヤー雛形
    [SerializeField]private GameObject EnemyBird = default;     //エネミー雛形

    //Animation
    private Animator PlayerAnim = null;
    private Animator EnemyAnim = null;

    //UI関係
    public GameObject GamePanel = default;
    public GameObject EnemyStatus = default;
    public GameObject GameOverPanel = default;
    public GameObject GiveUp = default;
    private bool isSwitch;      //表示切り替えをするか？
    [SerializeField]private List<Text> statusList = default;
    [SerializeField]private List<Text> enemyStatusList = default;
    [SerializeField]private List<Button> powerList = default;
    [SerializeField]private List<Button> diffenceList = default;
    [SerializeField]private List<Button> healList= default;

    //--------------------初期設定-------------------//

    void Start(){
        enemyLevel = 1;
        interval = 1.0f;
        //ステータス状況を初期化
        ResetStatusField();
        //プレイヤー生成
        player = new Bird(10,1,0,1,5);
        playerObj = Instantiate(PlayerBird);
        PlayerAnim = playerObj.GetComponent<Animator>();
        //初期エネミー生成
        enemy = new Bird(9,1,0,0,1);
        enemyObj = Instantiate(EnemyBird);
        EnemyAnim = enemyObj.GetComponent<Animator>();
        EnemyAnim.Play("BirdAdmin",0,0f);
        //ステータス表示（画面上）
        ChangeStatusText();
        EnemyChangeStatusText();

        
        GameOverPanel.SetActive(false);
        EnemyStatus.SetActive(false);
        GiveUp.SetActive(false);
    }

    //--------------------バトルフェーズ-------------------//

    void Update(){
        if(start){
            PlayerAnim.SetBool("isKick",true);
            EnemyAnim.SetBool("isKick",true);
            interval -= Time.deltaTime;
            if(interval < 0){
                interval = 1.0f;
                BattlePhase();
            }
        }
        
    }
    //攻撃メソッド
    void BattlePhase(){
        //相手の攻撃
        player.Damage(enemy.Attack);
        if(player.Physical <= 0){
            PlayerAnim.Play("BirdKnockDown",0,0f);
            start = false;                  //バトル終了
            GameOverPanel.SetActive(true);  //ゲームオーバー画面表示
            EnemyAnim.SetBool("isKick",false);
        }
        //プレイヤーの攻撃
        enemy.Damage(player.Attack);
        if(enemy.Physical <= 0){
            EnemyAnim.SetBool("isKick",false);
            player.StatusUp(5);     //ポイント増加
            player.SaveStatus();    //プレイヤーステータス保存
            ResetStatusField();     //ステータスフィールド初期化
            start = false;          //バトル終了
            GiveUp.SetActive(false);//中断ボタン非表示
            PlayerAnim.SetBool("isKick",false);
            StartCoroutine(NextEnemy());
        }
        if(0 < enemy.Cost){
            enemy.StatusUp(Random.Range(1,5));
        }
        //ステータス更新
        ChangeStatusText();
        EnemyChangeStatusText();
    }

    //エネミー生成(Animationイベント)
    IEnumerator NextEnemy(){
        EnemyAnim.Play("BirdKnockDown",0,0f);
        yield return new WaitForSeconds(1f);
        Destroy(enemyObj);
        enemyLevel++;
        enemy = new Bird(
                    Random.Range(9 * (enemyLevel - 1),9 * enemyLevel),
                    Random.Range(player.Attack,player.Attack + 2),
                    Random.Range(player.Guard,player.Guard + 2),
                    enemyLevel,enemyLevel);
        enemyObj = Instantiate(EnemyBird);
        EnemyAnim = enemyObj.GetComponent<Animator>();
        EnemyChangeStatusText();
        EnemyAnim.Play("BirdAdmin",0,0f);
        yield break;
    }

    //--------------------UI周りの処理-------------------//

    //ステータスの状況を初期化
    void ResetStatusField(){
        powerLevel = 0;
        diffenceLevel = 0;
        healLevel = 0;
        for(int i = 1;i < 5;i++){
            powerList[i].interactable = false;
            diffenceList[i].interactable = false;
            healList[i].interactable = false;
        }
    }
    //プレイヤーのステータスを表示＆更新
    void ChangeStatusText(){
        statusList[0].text = "HP:      "+player.Physical;
        statusList[1].text = "Power:   "+player.Attack;
        statusList[2].text = "Diffence:"+player.Guard;
        statusList[3].text = "Heal:    "+player.Lest;
        statusList[4].text = "Point:   "+player.Cost;
    }
    //エネミーのステータスを表示＆更新
    void EnemyChangeStatusText(){
        enemyStatusList[0].text = "HP:      "+enemy.Physical;
        enemyStatusList[1].text = "Power:   "+enemy.Attack;
        enemyStatusList[2].text = "Diffence:"+enemy.Guard;
        enemyStatusList[3].text = "Heal:    "+enemy.Lest;
        enemyStatusList[4].text = "Point:   "+enemy.Cost;
    }

    //--------------------Button関係のメソッド-------------------//
    public void SwitchPanel(){
        isSwitch = !isSwitch;
        if(isSwitch){
            GamePanel.SetActive(false);
            EnemyStatus.SetActive(true);
        }else if(!isSwitch){
            GamePanel.SetActive(true);
            EnemyStatus.SetActive(false);
        }
    }

    public void StatusUp(int status){
        try{
            //ポイントが０出ないなら
            if(0 < player.Cost){
                switch(status){
                    //攻撃力強化
                    case 1:
                        powerLevel++;
                        powerList[powerLevel].interactable = true;
                        player.StatusUp(status);
                        break;
                    //防御力強化
                    case 2:
                        diffenceLevel++;
                        diffenceList[diffenceLevel].interactable = true;
                        player.StatusUp(status);
                        break;
                    //回復力強化
                    case 3:
                        healLevel++;
                        healList[healLevel].interactable = true;
                        player.StatusUp(status);
                        break;
                    //回復
                    case 4:
                        player.StatusUp(status);
                        break;
                    default:
                        break;
                }
            }
        }catch{
            Debug.Log("起動中");
        }
        ChangeStatusText();
    }
    //ステータス初期化メソッド
    public void Reset(){
        player.Reset();
        ResetStatusField();
        ChangeStatusText();
    }
}
