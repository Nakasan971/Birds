using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    int action;
    float total;
    Vector3 targetPos;
    [SerializeField]private bool isLanding;
    [SerializeField]private List<float> actionList = default; 
    [SerializeField]private SpriteRenderer sprite = default;
    [SerializeField]private Animator anim = null;

    [SerializeField]private FoodScript foods = default;

    void Start(){
        foreach(float elem in actionList) total += elem;
        StartCoroutine(Movement()); 
    }
    int SelectAction(){
        float random = Random.value * total;
        for(int i = 0;i < actionList.Count;i++){
            if(random < actionList[i]) return i;
            else random -= actionList[i];
        }
        return 0;
    }
    IEnumerator Movement(){
        if(isLanding){
            float x = Random.Range(-5.0f,5.0f);
            float y = Random.Range(-4.0f,0.0f);
            yield return StartCoroutine(Moving(new Vector3(x,y,-1.0f),5f,"isLanding"));
            yield return new WaitForSeconds(3);
            isLanding = false;
            Debug.Log("Landing終了");
        }
        while(true){
            action = SelectAction();
            float x = Random.Range(-5.0f,5.0f);
            float y = Random.Range(-4.0f,0.0f);
            float speed = Random.Range(1.0f,5.0f);
            switch(action){
                case 1:
                    yield return StartCoroutine(Moving(new Vector3(x,y,-1.0f),speed,"isWalk"));
                    Debug.Log("Walking終了");
                    break;
                case 2:
                    yield return StartCoroutine(Moving(foods.FoodPos,speed,"isWalk"));
                    yield return StartCoroutine(PlayAnimation(1,"isEat"));
                    yield return StartCoroutine(Moving(new Vector3(x,y,-1.0f),speed,"isWalk"));
                    Debug.Log("Eating終了");
                    break;
                case 3:
                    yield return StartCoroutine(Moving(foods.WaterPos,speed,"isWalk"));
                    yield return StartCoroutine(PlayAnimation(2.2f,"isDrink"));
                    yield return StartCoroutine(Moving(new Vector3(x,y,-1.0f),speed,"isWalk"));
                    Debug.Log("Drinking終了");
                    break;
                case 4:
                    yield return StartCoroutine(PlayAnimation(10,"isSleep"));
                    Debug.Log("Sleeping終了"); 
                    break;
                case 5:
                    targetPos = new Vector3(transform.position.x + 4f,transform.position.y,-1.0f);
                    yield return StartCoroutine(Moving(targetPos,speed,"isWalk"));
                    targetPos = new Vector3(9f,Random.Range(4.0f,9.0f),-1.0f);
                    yield return StartCoroutine(Moving(targetPos,5.0f,"isTakeOff"));
                    Debug.Log("TakeOff終了");
                    Destroy(this.gameObject); 
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(Random.Range(10,25));
        }
    }
    IEnumerator Moving(Vector3 targetPos,float speed,string animName){
        float distance = targetPos.x - transform.position.x;
        if(distance < 0)sprite.flipX = true;
        else sprite.flipX = false;
        anim.SetBool(animName,true);
        while(transform.position != targetPos){
            transform.position = Vector3.MoveTowards(transform.position,targetPos,speed*Time.deltaTime);
            yield return null;
        }
        anim.SetBool(animName,false);
        yield break;
    }
    IEnumerator PlayAnimation(float sec,string animName){
        anim.SetBool(animName,true);
        yield return new WaitForSeconds(sec);
        anim.SetBool(animName,false);
    }
}
