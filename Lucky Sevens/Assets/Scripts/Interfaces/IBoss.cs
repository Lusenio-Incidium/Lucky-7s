public interface IBoss
{
    string bossName { get; set; }
    void startBoss();
    void attackPhase(int phase);
    void stunPhase();


    float onDamage(float amount, float currHP);
    int phaseUpdate(float hpAmount);

}
