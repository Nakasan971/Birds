using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour{
    int id = 0;     //番号付
    int range = 20; //出現間隔(最大)

    //GameObject
    [SerializeField] private GameObject food = default;
    [SerializeField] private GameObject water = default;
    [SerializeField] private GameObject Bird = default;
    [SerializeField] private List<GameObject> birdList = default;

    void Start(){
        Instantiate(food).name = "Food";        //フード生成
        Instantiate(water).name = "Water";      //水生成
        for(int i = 0;i < 10;i++) Spone(Bird);  //鳥生成
        StartCoroutine(FieldEvent());           //コルーチン生成
    }
    //鳥乱数生成メソッド
    IEnumerator FieldEvent(){
        while(true){
            int random = Random.Range(1,range);
            yield return new WaitForSeconds(10f);
            if(random == range - 1){
                Spone(Bird);
                range = 20;
                Debug.Log("出現!");
            }else{
                range -= random;
                Debug.Log("出現まで" + range);
            }
        }
    }
    //鳥生成メソッド
    void Spone(GameObject obj){
        GameObject prefab = Instantiate(obj);
        prefab.GetComponent<BirdScript>().enabled =true;
        prefab.name = ("Bird:"+id);
        birdList.Add(prefab);
        id++;
    }
}
