﻿//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework.Job
{
    /// <summary>
    /// 条件任务。
    /// </summary>
    public sealed class ConditionalJob : JobBase
    {
        private Predicate<ConditionalJob> m_Condition;
        private GameFrameworkAction<ConditionalJob, string> m_CompleteAction;
        private GameFrameworkAction<ConditionalJob, string> m_FailureAction;
        private GameFrameworkAction<ConditionalJob, string> m_CancelAction;

        /// <summary>
        /// 初始化条件任务的新实例。
        /// </summary>
        public ConditionalJob()
        {
            m_Condition = null;
            m_CompleteAction = null;
            m_FailureAction = null;
            m_CancelAction = null;
        }

        /// <summary>
        /// 设置任务完成的条件。
        /// </summary>
        /// <param name="condition">任务完成的条件。</param>
        public void SetCondition(Predicate<ConditionalJob> condition)
        {
            m_Condition = condition;
        }

        /// <summary>
        /// 设置任务完成时的行为。
        /// </summary>
        /// <param name="completeAction">任务完成时的行为。</param>
        public void SetCompleteAction(GameFrameworkAction<ConditionalJob, string> completeAction)
        {
            m_CompleteAction = completeAction;
        }

        /// <summary>
        /// 设置任务失败时的行为。
        /// </summary>
        /// <param name="failureAction">任务失败时的行为。</param>
        public void SetFailureAction(GameFrameworkAction<ConditionalJob, string> failureAction)
        {
            m_FailureAction = failureAction;
        }

        /// <summary>
        /// 设置任务取消时的行为。
        /// </summary>
        /// <param name="cancelAction">任务取消时的行为。</param>
        public void SetCancelAction(GameFrameworkAction<ConditionalJob, string> cancelAction)
        {
            m_CancelAction = cancelAction;
        }

        /// <summary>
        /// 任务开始时调用。
        /// </summary>
        protected internal override void OnStart()
        {
            base.OnStart();

            if (m_Condition == null)
            {
                OnFailure("Condition is invalid.");
                return;
            }

            if (m_CompleteAction == null)
            {
                OnFailure("Complete action is invalid.");
                return;
            }
        }

        /// <summary>
        /// 任务轮询时调用。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (!m_Condition(this))
            {
                return;
            }

            OnComplete("Conditional reach");
        }

        /// <summary>
        /// 任务完成时调用。
        /// </summary>
        /// <param name="reason">任务完成的原因。</param>
        protected internal override void OnComplete(string reason)
        {
            base.OnComplete(reason);

            m_CompleteAction(this, reason);
        }

        /// <summary>
        /// 任务失败时调用。
        /// </summary>
        /// <param name="reason">任务失败的原因。</param>
        protected internal override void OnFailure(string reason)
        {
            base.OnFailure(reason);

            if (m_FailureAction != null)
            {
                m_FailureAction(this, reason);
            }
        }

        /// <summary>
        /// 任务取消时调用。
        /// </summary>
        /// <param name="reason">任务取消的原因。</param>
        protected internal override void OnCancel(string reason)
        {
            base.OnCancel(reason);

            if (m_CancelAction != null)
            {
                m_CancelAction(this, reason);
            }
        }
    }
}
