using System.Collections.Generic;
using UnityEngine;

public class StatusManager
{
    private Health _owner;
    private List<IStatus> _statuses = new List<IStatus>();

    public StatusManager(Health owner)
    {
        _owner = owner;
    }

    // 상태이상 부여
    public void Apply(IStatus newStatus)
    {
        // 이미 같은 상태이상이 있으면 스택 증가
        foreach (var status in _statuses)
        {
            if (status.Name == newStatus.Name)
            {
                status.Stack += newStatus.Stack;
                Debug.Log($"{status.Name} 스택 증가 → {status.Stack}");
                return;
            }
        }

        // 없으면 새로 추가
        _statuses.Add(newStatus);
        newStatus.OnApply(_owner);
        Debug.Log($"{newStatus.Name} {newStatus.Stack} 부여");
    }

    public void OnTurnStart()
    {
        foreach (var status in _statuses)
            status.OnTurnStart(_owner);

        RemoveExpired();
    }

    public void OnTurnEnd()
    {
        foreach (var status in _statuses)
            status.OnTurnEnd(_owner);

        RemoveExpired();
    }

    // 스택 0 이하인 상태이상 제거
    private void RemoveExpired()
    {
        for (int i = _statuses.Count - 1; i >= 0; i--)
        {
            if (_statuses[i].Stack <= 0)
            {
                _statuses[i].OnRemove(_owner);
                _statuses.RemoveAt(i);
            }
        }
    }

    public List<IStatus> GetStatuses() => _statuses;

    public int GetStack(string statusName)
    {
        foreach (var status in _statuses)
            if (status.Name == statusName) return status.Stack;
        return 0;
    }
}