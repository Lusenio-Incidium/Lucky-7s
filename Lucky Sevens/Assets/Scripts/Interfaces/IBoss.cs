public interface IBoss
{
    void startBoss();
    void attackPhase(int phase);
    void stunPhase();


    int onDamage(int amount, int currHP);
    int phaseUpdate(int hpAmount);

}
