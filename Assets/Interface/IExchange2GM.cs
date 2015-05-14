
public interface IExchange2GM
{

    bool OpenAllow();

    void CreateExchange(string ratio, int score);

    void BalanceExchange(bool needclosegui);

    void CashoutQuit();
}
