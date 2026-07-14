using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice Ball effect", menuName = "Data/Item Effect/Ice Ball")]
public class IceBall_Effect : ItemEffect
{
    [SerializeField] private GameObject iceBallPrefab;
    [SerializeField] private Vector2 velocity;


    public override void ExecuteEffect(Transform _respondPosition)
    {
        Player player = PlayerManager.instance.player;

        bool thridAttack = player.primaryAttackState.comboCounter == 2;

        if (thridAttack)
        {
            GameObject newIceBall = Instantiate(iceBallPrefab, _respondPosition.position, player.transform.rotation);
            newIceBall.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * player.facingDirection, 0);
            Destroy(newIceBall, 2f);
        }


    }
}
