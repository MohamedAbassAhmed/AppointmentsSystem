using BackEnd.Data;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BackEnd.Core
{
    public class Services
    {
        ApplicationDBContext _db;
        ExceptionHandeller EXH;
        CultureInfo provider = CultureInfo.InvariantCulture;

        public Services(ApplicationDBContext db, ExceptionHandeller exh)
        {
            _db = db;
            EXH = exh;
        }
        public (bool Result, string Message) SaveSetting(string name, string value)
        {
            try
            {
                var setting = _db.Settings.FirstOrDefault(setting => setting.Name == name);
                if (setting == null)
                    _db.Settings.Add(new Setting() { Name = name, Value = value });

                else
                {
                    setting.Value = value;
                    _db.Entry<Setting>(setting).State = EntityState.Modified;
                }
                _db.SaveChanges();
                return (true, "");
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, Constants.ExceptionOccoured);
            }
        }
        public (bool Result, string Message) SaveSettings(SettingsModel model)
        {
            try
            {

                var session = _db.Settings.FirstOrDefault(setting => setting.Name == Constants.SessionPeriodSetting);
                if (session == null)
                    _db.Settings.Add(new Setting() { Name = Constants.SessionPeriodSetting, Value = model.SessionPeriod });

                else
                {
                    session.Value = model.SessionPeriod;
                    _db.Entry<Setting>(session).State = EntityState.Modified;
                }
                var wait = _db.Settings.FirstOrDefault(setting => setting.Name == Constants.WaitingPeriodSetting);
                if (wait == null)
                    _db.Settings.Add(new Setting() { Name = Constants.WaitingPeriodSetting, Value = model.WaitPeriod });

                else
                {
                    wait.Value = model.WaitPeriod;
                    _db.Entry<Setting>(wait).State = EntityState.Modified;
                }
                var rest = _db.Settings.FirstOrDefault(setting => setting.Name == Constants.RestPeriodSetting);
                if (rest == null)
                    _db.Settings.Add(new Setting() { Name = Constants.RestPeriodSetting, Value = model.RestPeriod });

                else
                {
                    rest.Value = model.RestPeriod;
                    _db.Entry<Setting>(rest).State = EntityState.Modified;
                }
                _db.SaveChanges();
                return (true, "");
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, Constants.ExceptionOccoured);
            }
        }

        public (bool Result, SettingsModel Settings) GetSettings()
        {
            try
            {
                SettingsModel model = new SettingsModel();
                model.SessionPeriod = GetSetting(Constants.SessionPeriodSetting);
                model.WaitPeriod = GetSetting(Constants.WaitingPeriodSetting);
                model.RestPeriod = GetSetting(Constants.RestPeriodSetting);
                return (true, model);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, null);
            }
        }
        public string GetSetting(string name)
        {
            try
            {
                var setting = _db.Settings.FirstOrDefault(s => s.Name == name);
                if (setting == null)
                {
                    return "";
                }
                return setting.Value;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return "";
            }
        }

        public (bool Result, DateTime AppointmentDate) CreateAppointment(AppointmentRequest model)
        {
            try
            {
                var tryOntheStart = TryOnTheStart(model);
                if (tryOntheStart.Result)
                {
                    return tryOntheStart;
                }
                return TryAfterLastAppointment(model);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        public (bool Result ,List<AppointmentModel> Appointments) GetAppointments()
        {
            try
            {
                var appointments = _db.Appointments.Select(a => new AppointmentModel()
                {
                    End = a.End.ToString("dd-MM-yyyy HH:mm"),
                    Start = a.Start.ToString("dd-MM-yyyy HH:mm"),
                    PatintName = a.PatintName
                }).ToList();
                return (true, appointments);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }

        public (bool Result, DateTime AppointmentDate) TryOnTheStart(AppointmentRequest model)
        {
            try
            {
                //var start = DateTime.ParseExact(model.Start, "d-M-yyyy", provider);
                var start = model.Start;
                var openingTime = start.Date.AddHours(8).AddMinutes(30);

                var sessionPeriod = Convert.ToInt32(_db.Settings.FirstOrDefault(s => s.Name == Constants.SessionPeriodSetting).Value);
                var waitPeriod = Convert.ToInt32(_db.Settings.FirstOrDefault(s => s.Name == Constants.WaitingPeriodSetting).Value);
                var newAppointmentEnd = openingTime.AddMinutes(sessionPeriod).AddMinutes(waitPeriod);

                var collesionAppointment = _db.Appointments.FirstOrDefault(a => a.Start < newAppointmentEnd);
                if (collesionAppointment != null)
                {
                    return (false, new());
                }
                else
                {
                    var newAppointment = new Appointment
                    {
                        Start = openingTime,
                        End = openingTime.AddMinutes(sessionPeriod),
                        PatintName = model.PatintName
                    };
                    _db.Appointments.Add(newAppointment);
                    _db.SaveChanges();
                    return (true, openingTime);
                }
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }

        public (bool Result, DateTime AppointmentDate) TryAfterLastAppointment(AppointmentRequest model)
        {
            try
            {
                //var start = DateTime.ParseExact(model.Start, "d-M-yyyy", provider);
                //var end = DateTime.ParseExact(model.End, "d-M-yyyy", provider);
                var start = model.Start;
                var end = model.End;

                var waitPeriod = Convert.ToInt32(_db.Settings.FirstOrDefault(s => s.Name == Constants.WaitingPeriodSetting).Value);
                var sessionPeriod = Convert.ToInt32(_db.Settings.FirstOrDefault(s => s.Name == Constants.SessionPeriodSetting).Value);
                //Appointments_In_TheTimeFrame
                var AITF = _db.Appointments.Where(a => a.Start > start && a.End < end).OrderBy(a => a.Start).ToArray();
                for (int i = 0; i < AITF.Length; i++)
                {
                    //if (AITF[i].Start.Hour < 14)
                    //{//firstPeriod
                    var endOfTheFirstPeriod = AITF[i].Start.Date.AddHours(14);
                    if (AITF[i].End.AddMinutes(waitPeriod).AddMinutes(sessionPeriod) < endOfTheFirstPeriod)
                    {
                        if (i + 1 == AITF.Length || AITF[i].End.AddMinutes(waitPeriod * 2).AddMinutes(sessionPeriod) < AITF[i + 1].Start)
                        {//avelable Frame Found
                            var newAppointment = new Appointment
                            {
                                Start = AITF[i].End.AddMinutes(waitPeriod),
                                End = AITF[i].End.AddMinutes(waitPeriod).AddMinutes(sessionPeriod),
                                PatintName = model.PatintName
                            };
                            _db.Appointments.Add(newAppointment);
                            _db.SaveChanges();
                            return (true, newAppointment.Start);
                        }
                        else
                        {
                            var restPeriod = Convert.ToInt32(_db.Settings.FirstOrDefault(s => s.Name == Constants.RestPeriodSetting).Value);

                            if (AITF[i].End.AddMinutes(waitPeriod).AddMinutes(sessionPeriod) >= endOfTheFirstPeriod &&
                                AITF[i].End.Date.AddHours(14).AddMinutes(restPeriod).AddMinutes(sessionPeriod).AddMinutes(waitPeriod) < AITF[i + 1].Start)
                            {//appointment can be set after the rest gap 
                                var newAppointment = new Appointment
                                {
                                    Start = AITF[i].End.Date.AddHours(14).AddMinutes(restPeriod),
                                    End = AITF[i].End.Date.AddHours(14).AddMinutes(restPeriod).AddMinutes(sessionPeriod),
                                    PatintName = model.PatintName
                                };
                                _db.Appointments.Add(newAppointment);
                                _db.SaveChanges();
                                return (true, newAppointment.Start);
                            }

                        }
                    }
                    //}
                    //else
                    //{//secondPeriod
                    var endOfTheSecondPeriod = AITF[i].Start.Date.AddHours(24);
                    DateTime startPoint;
                    if (AITF[i].End.Date.AddHours(14) > AITF[i].End)
                    {//this appointment(AITF[i]) ends befor the rest gap and no time befor gap to setup the nre appointment 
                        startPoint = AITF[i].End.Date.AddHours(14);
                        if (startPoint.AddMinutes(waitPeriod).AddMinutes(sessionPeriod) < endOfTheSecondPeriod)

                            if (AITF.Length == i + 1 || AITF[i].End.AddMinutes(waitPeriod).AddMinutes(sessionPeriod) < AITF[i + 1].Start)
                            {//avelable Frame Found
                                var newAppointment = new Appointment
                                {
                                    Start = startPoint,
                                    End = startPoint.AddMinutes(sessionPeriod),
                                    PatintName = model.PatintName
                                };
                                _db.Appointments.Add(newAppointment);
                                _db.SaveChanges();
                                return (true, newAppointment.Start);
                            }
                    }
                    else
                    {
                         startPoint = AITF[i].End;
                         //endOfTheSecondPeriod = AITF[i].Start.Date.AddHours(24);
                        if (startPoint.AddMinutes(waitPeriod).AddMinutes(sessionPeriod) < endOfTheSecondPeriod)
                            if (AITF.Length == i + 1 || AITF[i].End.AddMinutes(waitPeriod * 2).AddMinutes(sessionPeriod) < AITF[i + 1].Start)
                            {//avelable Frame Found
                                var newAppointment = new Appointment
                                {
                                    Start = startPoint.AddMinutes(waitPeriod),
                                    End = startPoint.AddMinutes(waitPeriod).AddMinutes(sessionPeriod),
                                    PatintName = model.PatintName
                                };
                                _db.Appointments.Add(newAppointment);
                                _db.SaveChanges();
                                return (true, newAppointment.Start);
                            }
                    }
                    //}
                }
                /**there are two senrios can lead you here:
                /*the first: there are no gap in the frame entered by the patint.
                /*the second one: the last appointment in the system is befor the end of the frame but it's end is too close
                *to the end of the second peiod.
                 **/
                if (AITF.Last().End.Date < end)
                {//the last appointment is before the end of the frame 
                    var newAppointment = new Appointment
                    {
                        Start = AITF.Last().End.Date.AddDays(1).AddHours(8).AddMinutes(30),
                        End = AITF.Last().End.Date.AddDays(1).AddHours(8).AddMinutes(30).AddMinutes(sessionPeriod),
                        PatintName = model.PatintName
                    };
                    _db.Appointments.Add(newAppointment);
                    _db.SaveChanges();
                    return (true, newAppointment.Start);
                }
                return (false, new());
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
    }
}
