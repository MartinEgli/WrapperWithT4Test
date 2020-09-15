/* ---------------------------------------------------------------------------- *
 *		The code is generated by 'T4Wrap' C# proxy generator T4 template		*
 * ---------------------------------------------------------------------------- */

namespace Siemens.Sinumerik.Operate.Services.Proxies
{
	public class AlarmSvc : IAlarmSvc, System.IDisposable
	{
		protected Siemens.Sinumerik.Operate.Services.AlarmSvc _inner;

		#region Constructors
		public AlarmSvc()
		{
			_inner = new Siemens.Sinumerik.Operate.Services.AlarmSvc();
		}
		public AlarmSvc(System.String language)
		{
			_inner = new Siemens.Sinumerik.Operate.Services.AlarmSvc(language);
		}
		public AlarmSvc(System.String language, System.String server)
		{
			_inner = new Siemens.Sinumerik.Operate.Services.AlarmSvc(language, server);
		}
		public AlarmSvc(System.String language, System.String server, System.Int32 eventlistLength)
		{
			_inner = new Siemens.Sinumerik.Operate.Services.AlarmSvc(language, server, eventlistLength);
		}
		#endregion

		#region Properties
		public virtual Siemens.Sinumerik.Operate.Services.AlarmSource[] Sources
		{
			get { return _inner.Sources; }	
		}
		public virtual System.String Server
		{
			get { return _inner.Server; }	
		}
		#endregion
		#region Methods
		public virtual void Dispose()
		{
			_inner.Dispose();
		}
		public virtual System.Guid Subscribe(Siemens.Sinumerik.Operate.Services.AlarmListChanged cb)
		{
			return _inner.Subscribe(cb);
		}
		public virtual void UnSubscribe(Siemens.Sinumerik.Operate.Services.AlarmListChanged cb)
		{
			_inner.UnSubscribe(cb);
		}
		public virtual System.Guid SubscribeEventList(Siemens.Sinumerik.Operate.Services.AlarmEventListChanged cb)
		{
			return _inner.SubscribeEventList(cb);
		}
		public virtual void UnSubscribeEventList(Siemens.Sinumerik.Operate.Services.AlarmEventListChanged cb)
		{
			_inner.UnSubscribeEventList(cb);
		}
		public virtual System.Guid SubscribeEvents(Siemens.Sinumerik.Operate.Services.NewAlarmEvents cb)
		{
			return _inner.SubscribeEvents(cb);
		}
		public virtual void UnSubscribeEvents(Siemens.Sinumerik.Operate.Services.NewAlarmEvents cb)
		{
			_inner.UnSubscribeEvents(cb);
		}
		public virtual void SetTimeCancelAlarm(ref Siemens.Sinumerik.Operate.Services.Alarm alarm)
		{
			_inner.SetTimeCancelAlarm(ref alarm);
		}
		public virtual void SetCancelAlarm(ref Siemens.Sinumerik.Operate.Services.Alarm alarm)
		{
			_inner.SetCancelAlarm(ref alarm);
		}
		public virtual void SetHmiAlarm(ref Siemens.Sinumerik.Operate.Services.Alarm alarm)
		{
			_inner.SetHmiAlarm(ref alarm);
		}
		public virtual void SetAcknowledgeAlarm(ref Siemens.Sinumerik.Operate.Services.Alarm alarm)
		{
			_inner.SetAcknowledgeAlarm(ref alarm);
		}
		public virtual void AcknowledgeAlarm(Siemens.Sinumerik.Operate.Services.Alarm alarm)
		{
			_inner.AcknowledgeAlarm(alarm);
		}
		public virtual void ResetAlarm(Siemens.Sinumerik.Operate.Services.Alarm alarm)
		{
			_inner.ResetAlarm(alarm);
		}
		public virtual void SetWarning(ref Siemens.Sinumerik.Operate.Services.Alarm alarm)
		{
			_inner.SetWarning(ref alarm);
		}
		public virtual void ResetWarning(Siemens.Sinumerik.Operate.Services.Alarm alarm)
		{
			_inner.ResetWarning(alarm);
		}
		#endregion

		#region System.IDisposable Members
		void System.IDisposable.Dispose()
		{
			((System.IDisposable)_inner).Dispose();
		}
		#endregion
	}

	public interface IAlarmSvc
	{
		#region Properties
				
		Siemens.Sinumerik.Operate.Services.AlarmSource[] Sources
		{
			get;
				
		}
				
		System.String Server
		{
			get;
				
		}
		#endregion


		#region Methods
		void Dispose();			
		System.Guid Subscribe(Siemens.Sinumerik.Operate.Services.AlarmListChanged cb);			
		void UnSubscribe(Siemens.Sinumerik.Operate.Services.AlarmListChanged cb);			
		System.Guid SubscribeEventList(Siemens.Sinumerik.Operate.Services.AlarmEventListChanged cb);			
		void UnSubscribeEventList(Siemens.Sinumerik.Operate.Services.AlarmEventListChanged cb);			
		System.Guid SubscribeEvents(Siemens.Sinumerik.Operate.Services.NewAlarmEvents cb);			
		void UnSubscribeEvents(Siemens.Sinumerik.Operate.Services.NewAlarmEvents cb);			
		void SetTimeCancelAlarm(ref Siemens.Sinumerik.Operate.Services.Alarm alarm);			
		void SetCancelAlarm(ref Siemens.Sinumerik.Operate.Services.Alarm alarm);			
		void SetHmiAlarm(ref Siemens.Sinumerik.Operate.Services.Alarm alarm);			
		void SetAcknowledgeAlarm(ref Siemens.Sinumerik.Operate.Services.Alarm alarm);			
		void AcknowledgeAlarm(Siemens.Sinumerik.Operate.Services.Alarm alarm);			
		void ResetAlarm(Siemens.Sinumerik.Operate.Services.Alarm alarm);			
		void SetWarning(ref Siemens.Sinumerik.Operate.Services.Alarm alarm);			
		void ResetWarning(Siemens.Sinumerik.Operate.Services.Alarm alarm);			
		#endregion
	}

	public interface IAlarmSvcFactory
	{
		IAlarmSvc Create(params object[] args);
		IAlarmSvc Create();
		IAlarmSvc Create(System.String language);
		IAlarmSvc Create(System.String language, System.String server);
		IAlarmSvc Create(System.String language, System.String server, System.Int32 eventlistLength);
	}

	public class AlarmSvcFactory : IAlarmSvcFactory
	{
		public virtual IAlarmSvc Create(params object[] args)
		{
			return (IAlarmSvc)System.Activator.CreateInstance(typeof(AlarmSvc), args);
		}

		public virtual IAlarmSvc Create()
		{
			return new AlarmSvc(); 
		}
		public virtual IAlarmSvc Create(System.String language)
		{
			return new AlarmSvc(language); 
		}
		public virtual IAlarmSvc Create(System.String language, System.String server)
		{
			return new AlarmSvc(language, server); 
		}
		public virtual IAlarmSvc Create(System.String language, System.String server, System.Int32 eventlistLength)
		{
			return new AlarmSvc(language, server, eventlistLength); 
		}
	}
}
