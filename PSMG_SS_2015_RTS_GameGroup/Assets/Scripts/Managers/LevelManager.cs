﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AssetReferences;
using Assets.Scripts.Config;
using Assets.Scripts.Controllers.Characters.Enemies.Troll;
using Assets.Scripts.Controllers.Characters.Imps;
using Assets.Scripts.Controllers.Objects;
using Assets.Scripts.LevelScripts;
using Assets.Scripts.ParameterObjects;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    ///     The LevelManager is a subcompoment of the GameManager and is responsible for
    ///     loading levels.
    ///     It also loads and holds the GameObjects in a level that are of
    ///     interest for the interaction logic.
    /// </summary>
    public class LevelManager : MonoBehaviour, TrollController.ITrollControllerListener,
        GoalController.IGoalControllerListener
    {
        public static LevelManager Instance;
        private List<ILevelManagerListener> listeners;
        public LevelConfig CurrentLevelConfig { get; set; }
        public Level CurrentLevel { get; set; }
        public LevelEvents CurrentLevelEvents { get; set; }

        private bool isEndingLevel;

        void GoalController.IGoalControllerListener.OnGoalReachedByImp()
        {
            OnGoalReached();
        }

        public void OnGoalReached()
        {
            if (isEndingLevel) return;

            isEndingLevel = true;
            StartCoroutine(OnGoalReachedRoutine());
        }

        private IEnumerator OnGoalReachedRoutine()
        {
            ImpManager.Instance.SpawnCounter.Stop();
            ImpManager.Instance.Imps.ForEach(i => i.GetComponent<ImpAnimationHelper>().Play(AnimationReferences.ImpWinLoop));
            SoundManager.Instance.BackgroundMusic.PlayAsLast(SoundReferences.WonTheme);

            yield return new WaitForSeconds(10f);

            LoadNextLevel();
        }

        void TrollController.ITrollControllerListener.OnEnemyHurt(TrollController trollController)
        {
            trollController.UnregisterListener();
        }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            listeners = new List<ILevelManagerListener>();
            menuSceneListeners = new List<ILevelManagerMenuSceneListener>();
            narrativeSceneListeners = new List<ILevelManagerNarrativeSceneListener>();
            isEndingLevel = false;
        }

        public void RegisterListener(ILevelManagerListener listener)
        {
            listeners.Add(listener);
        }

        public void LoadLevel(LevelConfig config)
        {
            CurrentLevelConfig = config;
            Application.LoadLevel(config.Name);
        }

        public void LoadLevel(int level)
        {
            CurrentLevelNumber = level;
            LoadLevel(LevelConfig.Levels[level]);
        }

        public int CurrentLevelNumber { get; private set; }

        public void LoadNextLevel()
        {
            if (CurrentLevelNumber >= LevelConfig.Levels.Length - 1) return;

            Reset();
            LoadLevel(CurrentLevelNumber + 1);
        }

        private void Reset()
        {
            isEndingLevel = false;

            Destroy(CurrentLevelEvents);
        }

        public void OnLevelWasLoaded(int level)
        {
            switch (CurrentLevelConfig.Type)
            {
                case LevelConfig.LevelType.InGame:
                    LoadInGameLevel();
                    break;
                case LevelConfig.LevelType.Menu:
                    LoadMenuLevel();
                    break;
                case LevelConfig.LevelType.Narrative:
                    LoadNarrativeLevel();
                    break;
            }
        }

        private void LoadNarrativeLevel()
        {
            CurrentLevel = new Level
            {
                CurrentLevelConfig = CurrentLevelConfig
            };
            narrativeSceneListeners.ForEach(nsl => nsl.OnNarrativeLevelStarted(CurrentLevel));
        }

        private void LoadMenuLevel()
        {
            CurrentLevel = new Level
            {
                CurrentLevelConfig = CurrentLevelConfig
            };
            menuSceneListeners.ForEach(msl => msl.OnMenuLevelStarted(CurrentLevel));
        }

        public void LoadInGameLevel()
        {
            CurrentLevel = new Level
            {
                CurrentLevelConfig = CurrentLevelConfig,
                MainCamera = GameObject.FindGameObjectWithTag(TagReferences.MainCamera),
                TopMargin = GameObject.FindGameObjectWithTag(TagReferences.TopMargin),
                BottomMargin = GameObject.FindGameObjectWithTag(TagReferences.BottomMargin),
                LeftMargin = GameObject.FindGameObjectWithTag(TagReferences.LeftMargin),
                RightMargin = GameObject.FindGameObjectWithTag(TagReferences.RightMargin),
                Obstacles = GameObject.FindGameObjectsWithTag(TagReferences.Obstacle).ToList(),
                Start = GameObject.FindWithTag(TagReferences.LevelStart),
                Goal = GameObject.FindWithTag(TagReferences.LevelGoal),
                CheckPoints = GameObject.FindGameObjectsWithTag(TagReferences.LevelCheckPoint).ToList(),
                HighlightableObjects = GameObject.FindGameObjectsWithTag(TagReferences.HighlightableObject).ToList(),
                Enemies = GameObject.FindGameObjectsWithTag(TagReferences.EnemyTroll).ToList()
            };
            RegisterListeners();
            listeners.ForEach(l => l.OnLevelStarted(CurrentLevel));

            LoadLevelEvents();
        }

        private void LoadLevelEvents()
        {
            switch (CurrentLevel.CurrentLevelConfig.Name)
            {
                case SceneReferences.Level01Koboldingen:
                    CurrentLevelEvents = gameObject.AddComponent<Level01Events>();
                    break;
                case SceneReferences.Level02CherryTopMountains:
                    CurrentLevelEvents = gameObject.AddComponent<Level02Events>();
                    break;
                case SceneReferences.Level05CastleGlazeArrival:
                    CurrentLevelEvents = gameObject.AddComponent<Level05Events>();
                    // TODO
                    //SpecialEffectsManager.Instance.Water.SpawnWater(-5, 30, -3.5f, -10);
                    break;
                case SceneReferences.Level06CastleGlazeDungenon:
                    CurrentLevelEvents = gameObject.AddComponent<Level06Events>();
                    break;
            }
        }

        public void RegisterListeners()
        {
            RegisterEnemyListener();
            RegisterGoalListener();
        }

        private void RegisterEnemyListener()
        {
            CurrentLevel.Enemies.ForEach(x => x.GetComponent<TrollController>().RegisterListener(this));
        }

        private void RegisterGoalListener()
        {
            CurrentLevel.Goal.GetComponent<GoalController>().RegisterListener(this);
        }

        public interface ILevelManagerListener
        {
            void OnLevelStarted(Level level);
        }

        private List<ILevelManagerMenuSceneListener> menuSceneListeners;

        public void RegisterMenuSceneListener(ILevelManagerMenuSceneListener listener)
        {
            menuSceneListeners.Add(listener);
        }

        public interface ILevelManagerMenuSceneListener
        {
            void OnMenuLevelStarted(Level level);
        }

        public void RegisterNarrativeSceneListener(ILevelManagerNarrativeSceneListener listener)
        {
            narrativeSceneListeners.Add(listener);
        }

        private List<ILevelManagerNarrativeSceneListener> narrativeSceneListeners;

        public interface ILevelManagerNarrativeSceneListener
        {
            void OnNarrativeLevelStarted(Level level);
        }
    }

}