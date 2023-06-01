public interface IBoss
{
    string bossName { get; set; }
    void startBoss();
    void attackPhase(int phase);
    void stunPhase();
    void unStun();


    float onDamage(float amount, float currHP);
    void phaseUpdate();


}
