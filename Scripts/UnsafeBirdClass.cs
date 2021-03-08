using System.Collections;
using System.Collections.Generic;
using UnityEngine;

unsafe public class UnsafeBird{
    //各ステータス
    private int* Hp;         //体力
    private int* Power;      //攻撃力
    private int* Diffence;   //防御力
    private int* Heal;       //回復力
    private int* Point;      //強化ポイント
    private int*[] status;   //上記を一纏めにするもの

    //各ステータスのGetter
    public int* Physical{get{return Hp;}}
    public int* Attack  {get{return Power;}}
    public int* Guard   {get{return Diffence;}}
    public int* Lest    {get{return Heal;}}
    public int* Cost    {get{return Point;}}

    //コンストラクタ
    public UnsafeBird(int Hp,int Power,int Diffence,int Heal,int Point){
        this.Hp       = &Hp;
        this.Power    = &Power;
        this.Diffence = &Diffence;
        this.Heal     = &Heal;
        this.Point    = &Point;
        this.status   = new int*[]{&Hp,&Power,&Diffence,&Heal,&Point};
        GetStatus();
    }
    //ステータス出力メソッド
    void GetStatus(){
        Debug.Log(
            "HP:"      +*status[0]+" "+
            "Power:"   +*status[1]+" "+
            "Diffence:"+*status[2]+" "+
            "Heal:"    +*status[3]+" "+
            "Point:"   +*status[4]);
    }
    //ステータス強化メソッド
    public void StatusUp(int mode){
        StatusUpProcess(mode);
    }
    //ステータス強化処理メソッド
    void StatusUpProcess(int mode){
        Point--;
        switch(mode){
            //攻撃力強化
            case 1:
                PowerUp();
                break;
            //防御力強化
            case 2:
                DiffenceUp();
                break;
            //回復力強化
            case 3:
                HealUp();
                break;
            //回復メソッド
            case 4:
                Healing();
                break;
            //ポイント増加
            case 5:
                PointUp();
                break;
            default:
                Debug.Log("存在しないステータスです");
                break;
        }
    }
    void PowerUp(){
        Power++;
        Debug.Log("パワーアップ！:"+*Power);
    }
    void DiffenceUp(){
        Diffence++;
        Debug.Log("ディフェンスアップ！:"+*Diffence);
    }
    void HealUp(){
        Heal++;
        Debug.Log("ヒールアップ！:"+*Heal);
    }
    void PointUp(){
        Point += 6;
        Debug.Log("ポイントアップ！:"+*Point);
    }
    void Healing(){
        Hp += *Heal;
        Debug.Log("回復："+*Hp);
    }
    //負傷メソッド
    public void Damage(int damage){
        if(0 < *Hp){
            if(damage - *Diffence <= 0)damage = 0;
            else damage = damage - *Diffence;
            *Hp -= damage;
            Debug.Log("残りのHP："+*Hp);
        }
    }
    //ステータス保存メソッド
    public void SaveStatus(){
        *status[1] = *Power;
        *status[2] = *Diffence;
        *status[3] = *Heal;
        *status[4] = *Point;
        Debug.Log("ステータス保存");
    }
    //ステータス初期化メソッド
    public void Reset(){
        *Power    = *status[1];
        *Diffence = *status[2];
        *Heal     = *status[3];
        *Point    = *status[4];
        Debug.Log("ステータスリセット");
    }
}
