using System;

using UnityEngine;
using UnityEngine.UI;

public class ParameterInputUI : MonoBehaviour
{
	public event Action<int, int, int, float> OnBoardParametersChanged;
	public event Action<float> OnTimeStepChanged;
	public event Action<int> OnUnderpopulationThresholdChanged;
	public event Action<int> OnReproductionCriterionChanged;
	public event Action<int> OnOverpopulationThresholdChanged;

	[SerializeField] private InputField _widthInput;
	[SerializeField] private InputField _heightInput;
	[SerializeField] private InputField _depthInput;
	[SerializeField] private InputField _timeStepInput;
	[SerializeField] private InputField _underpopulationThresholdInput;
	[SerializeField] private InputField _reproductionCriterionInput;
	[SerializeField] private InputField _overpopulationThresholdInput;
	[SerializeField] private InputField _spawnRateInput;
	[SerializeField] private Button _applyBoardParametersButton;

	private void Awake()
	{
		_widthInput.text = GameConstants.DEFAULT_SIZE.ToString();
		_heightInput.text = GameConstants.DEFAULT_SIZE.ToString();
		_depthInput.text = GameConstants.DEFAULT_SIZE.ToString();
		_timeStepInput.text = GameConstants.DEFAULT_TIMESTEP.ToString();
		_underpopulationThresholdInput.text = GameConstants.DEFAULT_UNDERPOPULATION_THRESHOLD.ToString();
		_reproductionCriterionInput.text = GameConstants.DEFAULT_REPRODUCTION_CRITERION.ToString();
		_overpopulationThresholdInput.text = GameConstants.DEFAULT_OVERPOPULATION_THRESHOLD.ToString();
		_spawnRateInput.text = GameConstants.DEFAULT_SPAWN_RATE.ToString();

		_timeStepInput.onEndEdit.AddListener(UpdateTimeStep);
		_underpopulationThresholdInput.onEndEdit.AddListener(UpdateUnderpopulationThreshold);
		_reproductionCriterionInput.onEndEdit.AddListener(UpdateReproductionCriterion);
		_overpopulationThresholdInput.onEndEdit.AddListener(UpdateOverpopulationThreshold);
		_applyBoardParametersButton.onClick.AddListener(UpdateBoardParameters);
	}

	private void OnDestroy()
	{
		_timeStepInput.onEndEdit.RemoveListener(UpdateTimeStep);
		_underpopulationThresholdInput.onEndEdit.RemoveListener(UpdateUnderpopulationThreshold);
		_overpopulationThresholdInput.onEndEdit.RemoveListener(UpdateOverpopulationThreshold);
		_applyBoardParametersButton.onClick.RemoveListener(UpdateBoardParameters);
	}

	private void UpdateBoardParameters()
	{
		// get the values from the UI that are related to the board initialisation
		int width = int.Parse(_widthInput.text);
		int height = int.Parse(_heightInput.text);
		int depth = int.Parse(_depthInput.text);
		float spawnRate = float.Parse(_spawnRateInput.text);

		width = Mathf.Max(width, 1);
		height = Mathf.Max(height, 1);
		depth = Mathf.Max(depth, 1);
		spawnRate = Mathf.Clamp01(spawnRate);

		OnBoardParametersChanged?.Invoke(width, height, depth, spawnRate);
	}

	private void UpdateTimeStep(string timeStep)
	{
		float timeStepValue = float.Parse(timeStep);
		timeStepValue = Mathf.Max(timeStepValue, 0f);
		OnTimeStepChanged?.Invoke(timeStepValue);
	}

	private void UpdateUnderpopulationThreshold(string underpopulationThreshold)
	{
		int underpopulationThresholdValue = int.Parse(underpopulationThreshold);
		OnUnderpopulationThresholdChanged?.Invoke(underpopulationThresholdValue);
	}

	private void UpdateReproductionCriterion(string reproductionCriterion)
	{
		int reproductionCriterionValue = int.Parse(reproductionCriterion);
		OnReproductionCriterionChanged?.Invoke(reproductionCriterionValue);
	}

	private void UpdateOverpopulationThreshold(string overpopulationThreshold)
	{
		int overpopulationThresholdValue = int.Parse(overpopulationThreshold);
		OnOverpopulationThresholdChanged?.Invoke(overpopulationThresholdValue);
	}
	
}
