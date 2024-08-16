using BatchProcess.Api.Models.Entities;
using BatchProcess.Api.Repository;
using Newtonsoft.Json;

namespace BatchProcess.API.Services
{
    /// <summary>
    /// Service class responsible for handling batch process messages.
    /// </summary>
    public class BatchProcessMessage
    {
        private BapDto? _bap;
        private readonly BatchProcessBapDbRepo _batchProcessBap;
        private readonly BatchProcessBapNDbRepo _batchProcessBapN;
        private readonly ILogger<BatchProcessMessage> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchProcessMessage"/> class.
        /// </summary>
        /// <param name="batchProcessBap">The repository for BatchProcess BAP database.</param>
        /// <param name="batchProcessBapN">The repository for BatchProcess BAPN database.</param>
        /// <param name="logger">The logger instance.</param>
        public BatchProcessMessage(
            BatchProcessBapDbRepo batchProcessBap,
            BatchProcessBapNDbRepo batchProcessBapN,
            ILogger<BatchProcessMessage> logger
        )
        {
            _batchProcessBap = batchProcessBap;
            _batchProcessBapN = batchProcessBapN;
            _logger = logger;
        }

        /// <summary>
        /// Sends a start process message.
        /// </summary>
        /// <param name="code">(REQUIRED) The message code to be sent.</param>
        /// <param name="message">(REQUIRED) The message to be sent.</param>
        public async void StartProcessMessage(string code, string message)
        {
            try
            {
                var newGuid = Guid.NewGuid();

                if (!string.IsNullOrEmpty(code))
                {
                    _bap = await _batchProcessBap.CreateAsync(
                        new BapDto()
                        {
                            Bap_Id = newGuid,
                            Bap_Code = code, //"BAP-001"
                            Bap_State = (int)BatchStateEnums.StartProcess,
                            Bap_Started_DateTime = DateOnly.FromDateTime(DateTime.Now),
                            Bap_Cancelled_DateTime = null,
                            Bap_Finished_DateTime = null,
                            Bap_Failed_DateTime = null,
                            Bap_Session_Id = Guid.NewGuid(),
                            CreatedBy = "Tester",
                            Bap_BapNs = new List<BapnDto>() {
                                new BapnDto()
                                {
                                    BapN_Id = Guid.NewGuid(),
                                    BapN_BapId = newGuid,
                                    BapN_AA = 1,
                                    BapN_DateTime = DateOnly.FromDateTime(DateTime.Now),
                                    BapN_kind = 0,
                                    BapN_Data = message,
                                    CreatedBy = "Tester"
                                }
                            }
                        }
                    );

                    _logger.LogInformation($"Starting new [batch process] with id ({_bap.Bap_Id}).");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"StartProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}");
                throw new Exception(
                    $"StartProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}"
                );
            }
        }

        /// <summary>
        /// Sends an end process message.
        /// </summary>
        public async void EndProcessMessage()
        {
            try
            {
                IEnumerable<BapDto> bapList = await _batchProcessBap.FilterAsync(
                    b => (b.Bap_Id == _bap!.Bap_Id) &&
                         (b.Bap_Session_Id == _bap.Bap_Session_Id) &&
                         (b.Bap_State == (int)BatchStateEnums.InProgressProcess)
                );

                if (!bapList.Any())
                {
                    _logger.LogError($"EndProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: No active process found.");
                    throw new Exception(
                        $"EndProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: No active process found."
                    );
                }

                BapDto? bap = bapList.FirstOrDefault();
                bap!.Bap_State = (int)BatchStateEnums.EndProcess;
                bap!.Bap_Finished_DateTime = DateOnly.FromDateTime(DateTime.Now);

                await _batchProcessBap.UpdateAsync(b => b.Bap_Id == bap.Bap_Id, bap);

                _logger.LogInformation($"Finish [batch process] with id ({_bap?.Bap_Id}).");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"StartProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}");
                throw new Exception(
                    $"StartProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}"
                );
            }
        }

        /// <summary>
        /// Sends an in progress message.
        /// </summary>
        public async void InProgressProcessMessage()
        {
            try
            {
                IEnumerable<BapDto> bapList = await _batchProcessBap.FilterAsync(
                    b => (b.Bap_Id == _bap!.Bap_Id) &&
                         (b.Bap_Session_Id == _bap.Bap_Session_Id) &&
                         (b.Bap_State == (int)BatchStateEnums.StartProcess)
                );

                if (!bapList.Any())
                {
                    _logger.LogError($"EndProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: No start process found.");
                    throw new Exception(
                        $"EndProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: No start process found."
                    );
                }

                BapDto? bap = bapList.FirstOrDefault();
                bap!.Bap_State = (int)BatchStateEnums.InProgressProcess;

                await _batchProcessBap.UpdateAsync(b => b.Bap_Id == bap.Bap_Id, bap);

                _logger.LogInformation($"InProgress [batch process] with id ({_bap?.Bap_Id}).");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"InProgressProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}");
                throw new Exception(
                    $"InProgressProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}"
                );
            }
        }

        /// <summary>
        /// Sends a cancel message.
        /// </summary>
        public async void CancelProcessMessage()
        {
            try
            {
                IEnumerable<BapDto> bapList = await _batchProcessBap.FilterAsync(
                    b => (b.Bap_Id == _bap!.Bap_Id) &&
                        (b.Bap_Session_Id == _bap.Bap_Session_Id) &&
                        (b.Bap_State == (int)BatchStateEnums.InProgressProcess)
                );

                if (!bapList.Any())
                {
                    _logger.LogError($"EndProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: No start process found.");
                    throw new Exception(
                        $"EndProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: No start process found."
                    );
                }

                BapDto? bap = bapList.FirstOrDefault();
                bap!.Bap_State = (int)BatchStateEnums.InterruptProcess;
                bap!.Bap_Cancelled_DateTime = DateOnly.FromDateTime(DateTime.Now);

                await _batchProcessBap.UpdateAsync(b => b.Bap_Id == bap.Bap_Id, bap);

                _logger.LogInformation($"InProgress [batch process] with id ({_bap?.Bap_Id}).");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"InProgressProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}");
                throw new Exception(
                    $"InProgressProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}"
                );
            }
        }

        /// <summary>
        /// Sends a success message.
        /// </summary>
        public async void SuccessProcessMessage()
        {
            try
            {
                int bapNAA = await GetLastBapNAA() + 1;

                await _batchProcessBapN.CreateAsync(
                    new BapnDto()
                    {
                        BapN_Id = Guid.NewGuid(),
                        BapN_BapId = _bap!.Bap_Id,
                        BapN_BapDto = _bap,
                        BapN_AA = bapNAA,
                        BapN_DateTime = DateOnly.FromDateTime(DateTime.Now),
                        BapN_kind = 0,
                        BapN_Data = "Η δημιουργία διαδικασίας ολοκληρώθηκε επιτυχώς.",
                        CreatedBy = "Tester",
                    }
                );

                // Update Bap state
                IEnumerable<BapDto> bapList = await _batchProcessBap.FilterAsync(
                    b => (b.Bap_Id == _bap!.Bap_Id) &&
                         (b.Bap_Session_Id == _bap.Bap_Session_Id) &&
                         (b.Bap_State == (int)BatchStateEnums.InProgressProcess)
                );

                if (!bapList.Any())
                {
                    _logger.LogError($"EndProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: No in progress process found.");
                    throw new Exception(
                        $"EndProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: No in progress process found."
                    );
                }

                BapDto? bap = bapList.FirstOrDefault();
                bap!.Bap_State = (int)BatchStateEnums.EndProcess;
                bap!.Bap_Finished_DateTime = DateOnly.FromDateTime(DateTime.Now);

                await _batchProcessBap.UpdateAsync(b => b.Bap_Id == bap.Bap_Id, bap);

                _logger.LogInformation($"SuccessProcessMessage records: Η δημιουργία διαδικασίας ολοκληρώθηκε επιτυχώς.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"SuccessProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}");
                throw new Exception(
                    $"SuccessProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}"
                );
            }
        }

        /// <summary>
        /// Sends a failure message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        public async void FailureProcessMessage(string message)
        {
            try
            {
                int bapNAA = await GetLastBapNAA() + 1;

                await _batchProcessBapN.CreateAsync(
                    new BapnDto()
                    {
                        BapN_Id = Guid.NewGuid(),
                        BapN_BapId = _bap!.Bap_Id,
                        BapN_BapDto = _bap,
                        BapN_AA = bapNAA,
                        BapN_DateTime = DateOnly.FromDateTime(DateTime.Now),
                        BapN_kind = 0,
                        BapN_Data = $"Η δημιουργία διαδικασίας απέτυχε. - {message}",
                        CreatedBy = "Tester",
                    }
                );

                // Update Bap state
                IEnumerable<BapDto> bapList = await _batchProcessBap.FilterAsync(
                    b => (b.Bap_Id == _bap!.Bap_Id) &&
                         (b.Bap_Session_Id == _bap.Bap_Session_Id) &&
                         (b.Bap_State == (int)BatchStateEnums.InProgressProcess)
                );

                if (!bapList.Any())
                {
                    _logger.LogError($"FailureProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: No in progress process found.");
                    throw new Exception(
                        $"FailureProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: No in progress process found."
                    );
                }

                BapDto? bap = bapList.FirstOrDefault();
                bap!.Bap_State = (int)BatchStateEnums.FailureProcess;
                bap!.Bap_Failed_DateTime = DateOnly.FromDateTime(DateTime.Now);

                await _batchProcessBap.UpdateAsync(b => b.Bap_Id == bap.Bap_Id, bap);

                _logger.LogInformation($"FailureProcessMessage records: Η δημιουργία διαδικασίας απέτυχε.");
            }
            catch (System.Exception ex)
            {

                _logger.LogError($"FailureProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}");
                throw new Exception(
                    $"FailureProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}"
                );
            }
        }

        /// <summary>
        /// Sends a total count message.
        /// </summary>
        /// <param name="total">The total count of processes. Defaults to 0.</param>
        public async void TotalCountProcessMessage(int total = 0)
        {
            try
            {
                int bapNAA = await GetLastBapNAA() + 1;

                await _batchProcessBapN.CreateAsync(new BapnDto()
                {
                    BapN_Id = _bap!.Bap_Id,
                    BapN_BapId = _bap!.Bap_Id,
                    BapN_AA = bapNAA,
                    BapN_DateTime = DateOnly.FromDateTime(DateTime.Now),
                    BapN_kind = 0,
                    BapN_Data = $"Συνολικές εγγραφές προς επεξεργασία: {total}",
                    CreatedBy = "Tester"
                });

                _logger.LogInformation($"Total process records: {total}");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"TotalCountProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}");
                throw new Exception(
                    $"TotalCountProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}"
                );
            }
        }

        /// <summary>
        /// Sends a progress message.
        /// </summary>
        /// <param name="progress">The current progress of the process. Defaults to 0.</param>
        public async void ProgressProcessMessage(int progress = 0)
        {
            try
            {
                int bapNAA = await GetLastBapNAA() + 1;

                await _batchProcessBapN.CreateAsync(
                    new BapnDto()
                    {
                        BapN_Id = Guid.NewGuid(),
                        BapN_BapId = _bap!.Bap_Id,
                        BapN_BapDto = _bap,
                        BapN_AA = bapNAA,
                        BapN_DateTime = DateOnly.FromDateTime(DateTime.Now),
                        BapN_kind = 0,
                        BapN_Data = $"Επιτυχής καταχώρηση {progress} εγγραφών.",
                        CreatedBy = "Tester",
                    }
                );

                _logger.LogInformation($"ProgressProcess records: {progress}");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ProgressProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}");
                throw new Exception(
                    $"ProgressProcessMessage for {JsonConvert.SerializeObject(_bap)} - Error: {ex.Message}"
                );
            }
        }

        /// <summary>
        /// Gets the last(max) BapN AA.
        /// </summary>
        /// <returns>The max aa as integer</returns>
        private async Task<int> GetLastBapNAA()
        {
            // IEnumerable<BapDto> bapList = await _batchProcessBap.FilterAsNoTrackingAsync(
            //     b => (b.Bap_Id == _bap?.Bap_Id) &&
            //          (b.Bap_Session_Id == _bap.Bap_Session_Id) &&
            //          (b.Bap_State == (int)BatchStateEnums.InProgressProcess)
            // );

            // if (!bapList.Any())
            // {
            //     _logger.LogError($"GetLastBapNAA for {JsonConvert.SerializeObject(_bap)} - Error: No active process found.");
            //     throw new Exception(
            //         $"GetLastBapNAA for {JsonConvert.SerializeObject(_bap)} - Error: No active process found."
            //     );
            // }

            // BapDto? bap = bapList.FirstOrDefault();

            IEnumerable<BapnDto> bapnList = await _batchProcessBapN.FilterAsNoTrackingAsync(
                b => b.BapN_BapId == _bap!.Bap_Id
            );

            if (!bapnList.Any())
            {
                _logger.LogError($"GetLastBapNAA for {JsonConvert.SerializeObject(_bap)} - Error: No active process steps found.");
                throw new Exception(
                    $"GetLastBapNAA for {JsonConvert.SerializeObject(_bap)} - Error: No active process steps found."
                );
            }

            int? maxAA = bapnList.Max(b => b.BapN_AA);

            return maxAA ?? 0;
        }

    }
}
