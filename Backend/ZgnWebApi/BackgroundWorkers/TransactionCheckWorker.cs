using System.Net.Sockets;
using System.Net;
using System.Text;
using ZgnWebApi.Core.Utilities.IoC;
using ZgnWebApi.Entities;
using ZgnWebApi.Integrations.BlueBotics;
using Microsoft.AspNetCore.Http;

namespace ZgnWebApi.BackgroundWorkers
{
    public class TransactionCheckWorkerService : BackgroundService
    {
        private readonly IBlueBoticsIntegration _blueBoticsIntegration;

        public TransactionCheckWorkerService(IBlueBoticsIntegration blueBoticsIntegration)
        {
            _blueBoticsIntegration = blueBoticsIntegration;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    new Transaction().GetAll(e =>
                    e.ProcessId != null &&
                    e.ProcessId != "" &&
                    e.EndDate == null &&
                    e.Status != TransactionStatus.End.Value &&
                    e.Status != TransactionStatus.Cancelled.Value &&
                    e.Status != TransactionStatus.Error.Value &&
                    e.Status != TransactionStatus.End.Value).Data.ForEach(t =>
                    {
                        try
                        {
                            var mission = _blueBoticsIntegration.GetMission(t.ProcessId);
                            if (mission.RetCode != 0) return;
                            if (mission.Payload == null) return;
                            if (mission.Payload.Transportstate == 0) return;
                            string? lastStatus = t.Status;
                            switch (mission.Payload.Transportstate)
                            {
                                case 1:
                                    t.Status = TransactionStatus.Accepted.Value;
                                    break;
                                case 2:
                                    t.Status = TransactionStatus.Rejected.Value;
                                    break;
                                case 3:
                                    t.Status = TransactionStatus.Assigned.Value;
                                    break;
                                case 4:
                                    t.Status = TransactionStatus.Moving.Value;
                                    break;
                                case 5:
                                    t.Status = TransactionStatus.TransportingToSelector.Value;
                                    break;
                                case 6:
                                    t.Status = TransactionStatus.SelectingDeliveryFromStart.Value;
                                    break;
                                case 7:
                                    t.Status = TransactionStatus.Delivering.Value;
                                    break;
                                case 8:
                                    t.Status = TransactionStatus.Terminated.Value;
                                    break;
                                case 9:
                                    t.Status = TransactionStatus.Cancelled.Value;
                                    break;
                                case 10:
                                    t.Status = TransactionStatus.Error.Value;
                                    break;
                                case 11:
                                    t.Status = TransactionStatus.Cancelling.Value;
                                    break;
                                case 12:
                                    t.Status = TransactionStatus.SelectingPickUpNode.Value;
                                    break;
                                case 13:
                                    t.Status = TransactionStatus.SelectingDeliveryFromSelector.Value;
                                    break;
                                case 14:
                                    t.Status = TransactionStatus.MovingToDepartureSelector.Value;
                                    break;

                            }
                            if (lastStatus != t.Status)
                                t.UpdateStatus();
                        }
                        catch (Exception) { }
                    });
                    new Transaction().GetAll(e =>
                    (e.ProcessId == "" || e.ProcessId == null) &&
                    e.Status != TransactionStatus.Pending.Value &&
                    e.Status != TransactionStatus.End.Value &&
                    e.Status != TransactionStatus.Cancelled.Value &&
                    e.Status != TransactionStatus.Error.Value &&
                    e.EndDate == null
                    ).Data.ForEach(t =>
                    {
                        try
                        {
                            t.Start();
                        }
                        catch (Exception) { }
                    });
                    Thread.Sleep(_blueBoticsIntegration.GetDelay());
                }
            });
        }
    }
}
