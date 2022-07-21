using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOfLife : MonoBehaviour
{
	[SerializeField] private ParameterInputUI _parameterInputUI;
	[SerializeField] private GameObject _cellPrefab;

	private float _timeStep = GameConstants.DEFAULT_TIMESTEP;
	private int _underpopulationThreshold = GameConstants.DEFAULT_UNDERPOPULATION_THRESHOLD;
	private int _reproductionCriterion = GameConstants.DEFAULT_REPRODUCTION_CRITERION;
	private int _overpopulationThreshold = GameConstants.DEFAULT_OVERPOPULATION_THRESHOLD;
	private bool[,,] _board;
	private GameObject[,,] _cells;

	private void Awake()
	{
		// hook up events to UI
		_parameterInputUI.OnBoardParametersChanged += GenerateBoard;
		_parameterInputUI.OnTimeStepChanged += HandleTimeStepChanged;
		_parameterInputUI.OnUnderpopulationThresholdChanged += HandleUnderpopulationThresholdChanged;
		_parameterInputUI.OnReproductionCriterionChanged += HandleReproductionCriterionChanged;
		_parameterInputUI.OnOverpopulationThresholdChanged += HandleOverpopulationThresholdChanged;

		GenerateBoard(
			GameConstants.DEFAULT_SIZE,
			GameConstants.DEFAULT_SIZE,
			GameConstants.DEFAULT_SIZE,
			GameConstants.DEFAULT_SPAWN_RATE
		);
	}

	private void OnDestroy()
	{
		_parameterInputUI.OnBoardParametersChanged -= GenerateBoard;
		_parameterInputUI.OnTimeStepChanged -= HandleTimeStepChanged;
		_parameterInputUI.OnUnderpopulationThresholdChanged -= HandleUnderpopulationThresholdChanged;
		_parameterInputUI.OnOverpopulationThresholdChanged -= HandleOverpopulationThresholdChanged;
	}

	private void GenerateBoard(int width, int height, int depth, float spawnRate)
	{
		// clear the board if needed
		if (_cells != null)
		{
			foreach (GameObject cell in _cells)
			{
				Destroy(cell);
			}
		}

		// initialize empty arrays
		_board = new bool[width, height, depth];
		_cells = new GameObject[width, height, depth];
		
		// generate the board
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				for (int z = 0; z < depth; z++)
				{
					bool alive = Random.value < spawnRate;
					GameObject newCell = Instantiate(_cellPrefab);
					newCell.transform.parent = transform;
					newCell.transform.localPosition = new Vector3(x - width/2, y - height/2, z - depth/2);
					newCell.SetActive(alive);
					_board[x, y, z] = alive;
					_cells[x, y, z] = newCell;
				}
			}
		}

		// start the game loop
		StopAllCoroutines();
		StartCoroutine(UpdateBoardRoutine());
	}

	private IEnumerator UpdateBoardRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(_timeStep);

			for (int x = 0; x < _board.GetLength(0); x++)
			{
				for (int y = 0; y < _board.GetLength(1); y++)
				{
					for (int z = 0; z < _board.GetLength(2); z++)
					{
						UpdateCell(x, y, z);
					}
				}
			} 
		}
	}

	private void UpdateCell(int x, int y, int z)
	{
		int aliveNeighbours = GetAliveNeighbours(x, y, z);

		if (_board[x, y, z])
		{
			if (aliveNeighbours < _underpopulationThreshold || aliveNeighbours > _overpopulationThreshold)
			{
				// dead!
				_board[x, y, z] = false;
				_cells[x, y, z].SetActive(false);
			}
		}
		else
		{
			if (aliveNeighbours == _reproductionCriterion)
			{
				// new cell!
				_board[x, y, z] = true;
				_cells[x, y, z].SetActive(true);
			}
		}
	}

	private int GetAliveNeighbours(int x, int y, int z)
	{
		int aliveNeighbours = 0;

		// loop over all neighbor indeces
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				for (int k = -1; k <= 1; k++)
				{
					if (i == 0 && j == 0 && k == 0)
					{
						// Skip the cell itself
						continue;
					}

					int neighbourX = x + i;
					int neighbourY = y + j;
					int neighbourZ = z + k;

					if (neighbourX < 0 || neighbourX >= _board.GetLength(0))
					{
						// out of bounds
						continue;
					}

					if (neighbourY < 0 || neighbourY >= _board.GetLength(1))
					{
						// out of bounds
						continue;
					}

					if (neighbourZ < 0 || neighbourZ >= _board.GetLength(2))
					{
						// out of bounds
						continue;
					}

					bool alive = _board[neighbourX, neighbourY, neighbourZ];
					if (alive)
					{
						aliveNeighbours++;
					}
				}
			}
		}

		return aliveNeighbours;
	}

	private void HandleTimeStepChanged(float timeStep) => _timeStep = timeStep;
	private void HandleUnderpopulationThresholdChanged(int threshold) => _underpopulationThreshold = threshold;
	private void HandleReproductionCriterionChanged(int reproduction) => _reproductionCriterion = reproduction;
	private void HandleOverpopulationThresholdChanged(int threshold) => _overpopulationThreshold = threshold;
	

}
