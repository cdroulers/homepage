class AddPatientMutation
{
  public void AddPatient(IRepository repo, dynamic input)
  {
    Patient patient = PatientInputType.GetPatient(input.patient);

    string hospitalId = input.hospitalId, indexNumber = input.indexNumber;
    if (!string.IsNullOrWhiteSpace(hospitalId) && !string.IsNullOrWhiteSpace(indexNumber))
    {
      var hospital = await repo.LoadAsync<Hospital>(new AggregateId(hospitalId));
      patient.LinkOrUpdateHospital(hospital, indexNumber);
    }

    await repo.SaveAsync(patient);
  }
}
