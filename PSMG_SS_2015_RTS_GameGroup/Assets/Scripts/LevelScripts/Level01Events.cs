﻿using System.Linq;
using Assets.Scripts.AssetReferences;
using Assets.Scripts.Managers;

namespace Assets.Scripts.LevelScripts
{
    public class Level01Events : LevelEvents
    {
        public Event MapStartetMessage { get; private set; }
        public Event SelectImpMessageTrigger { get; private set; }
        public Event ShieldCarrierTrainedMessage { get; private set; }
        public Event LaddersCollectedMessage { get; private set; }
        public Event WeaponsCollectedMessage { get; private set; }
        public Event BombNeededMessage { get; private set; }
        public Event TrollMetMessage { get; private set; }
        public Event ProfessionAssignedMessage { get; private set; }
        public Event SuspensionBridgesCrossed { get; private set; }
        public Event LevelCompleted { get; private set; }

        protected override void RegisterEvents()
        {
            MapStartetMessage = gameObject.AddComponent<Event>();
            MapStartetMessage.Message =
                "Gebieter, Prinzessin Koboldigunde wurde entführt. König Krümelbart hat sie in sein tiefstes Verlies geworfen. Ihr müsst sie retten. Macht euch auf!!!";
            MapStartetMessage.Action = MapStartetAction;

            SelectImpMessageTrigger = Events.First(e => e.Nr == 2);
            SelectImpMessageTrigger.Message =
                "Mein Herr. Ihr könnt Kobolde auswählen, indem ihr auf sie klickt oder die Tab-Taste benutzt.";
            SelectImpMessageTrigger.Action = SelectImpAction;

            // TODO not played so far; should be played when the first imps is selected; create
            ProfessionAssignedMessage = gameObject.AddComponent<Event>();
            ProfessionAssignedMessage.Message = "Herrvorragend. Nun gebt mir was zu tun!";

            // TODO should be played when first coward is assigned
            ShieldCarrierTrainedMessage = gameObject.AddComponent<Event>();
            ShieldCarrierTrainedMessage.Message =
                "Hierhinter bin ich sicher, Meister. Nichts und niemand kommt an mir vorbei.";
            ShieldCarrierTrainedMessage.Action = ShieldCarrierTrainedAction;

            // TODO created
            SuspensionBridgesCrossed = Events.First(e => e.Nr == 5);
            SuspensionBridgesCrossed.Message =
                "Diese Hängebrücke sieht zerbrechlich aus, Gebieter. Ich weiß nicht, ob sie das Gewicht unserer Schilde trägt.";

            LaddersCollectedMessage = Events.First(e => e.Nr == 6);
            LaddersCollectedMessage.Message =
                "Meisterlich, Meister. Wir können die Leitern nutzen, um Hindernisse zu überqueren.";
            LaddersCollectedMessage.Action = LaddersCollectedAction;

            BombNeededMessage = Events.First(e => e.Nr == 7);
            BombNeededMessage.Message =
                "Ein dicker, fetter Brocken. Sprengt ihn, mein Herr. In unserer Waffenkammer liegen Bomben und Speere.";
            BombNeededMessage.Action = BombNeededAction;

            WeaponsCollectedMessage = Events.First(e => e.Nr == 8);
            WeaponsCollectedMessage.Message =
                "Sprengstoff und Speere. Nun sind wir gerüstet. Reißt alles nieder! Fels und Mauer, Tor und Turm.";
            WeaponsCollectedMessage.Action = WeaponsCollectedAction;

            TrollMetMessage = Events.First(e => e.Nr == 9);
            TrollMetMessage.Message =
                "Ein Troll … Achtsam, mein Herr. Diese Ungeheuer sind stark und rasch verärgert. Lasst uns aus der Deckung heraus angreifen, um das Biest auszuschalten";
            TrollMetMessage.Action = TrollMetAction;

            LevelCompleted = gameObject.AddComponent<Event>();
            LevelCompleted.Message =
                "Hmmm, auf dem Weg liegen Kuchenkrümel herum. Die Prinzessin muss hier vorbeigekommen sein. Ihr habt die Spur des arglistigen Entführers gefunden. Ein Meisterstück, eure Boshaftigkeit.";
            LevelCompleted.Action = LevelCompletedAction;
        }

        private void LevelCompletedAction()
        {
            // TODO
        }

        private void TrollMetAction()
        {
            SoundManager.Instance.Narrator.PlayAfterCurrent(SoundReferences.SoundLvl1_10);
        }

        private void BombNeededAction()
        {
            SoundManager.Instance.Narrator.PlayAfterCurrent(SoundReferences.SoundLvl1_08);
        }

        public void ShieldCarrierTrainedAction()
        {
            SoundManager.Instance.Narrator.PlayAfterCurrent(SoundReferences.SoundLvl1_04);
        }

        private void MapStartetAction()
        {
            SoundManager.Instance.Narrator.PlayAfterCurrent(SoundReferences.SoundLvl1_01);
        }

        private void SelectImpAction()
        {
            SoundManager.Instance.Narrator.PlayAfterCurrent(SoundReferences.SoundLvl1_02);
        }

        private void LaddersCollectedAction()
        {
            LevelManager.Instance.CurrentLevel.CurrentLevelConfig.MaxProfessions[2] += 2;
            ImpManager.Instance.NotifyMaxProfessions();
            SoundManager.Instance.Narrator.PlayAfterCurrent(SoundReferences.SoundLvl1_07);
        }

        private void WeaponsCollectedAction()
        {
            LevelManager.Instance.CurrentLevel.CurrentLevelConfig.MaxProfessions[0] += 1;
            LevelManager.Instance.CurrentLevel.CurrentLevelConfig.MaxProfessions[3] += 2;
            ImpManager.Instance.NotifyMaxProfessions();
            SoundManager.Instance.Narrator.PlayAfterCurrent(SoundReferences.SoundLvl1_09);
        }
    }
}