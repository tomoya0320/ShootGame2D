/// <summary>
/// 用来计算子弹攻击的间隔
/// </summary>
public class Timer {
  private float m_deltaTime;
  private float m_startTime;
  private float m_currentTime;
  public bool ready => m_currentTime >= m_startTime + m_deltaTime;

  public void OnStart(float startTime, float deltaTime) {
    m_deltaTime = deltaTime;
    m_startTime = startTime;
    m_currentTime = startTime;
  }

  public void OnUpdate(float delta) {
    m_currentTime += delta;
  }

  public void OnReset() {
    m_deltaTime = m_startTime = m_currentTime = 0;
  }
}