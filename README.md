# Roguelike_Tane_Practice

Unity 2Dローグライク練習プロジェクトです。

## 現在の実装状態（遊べる範囲）

- グリッド移動（`WASD` / 矢印キー）
- ターン制進行（プレイヤー行動 -> 敵行動）
- 基本戦闘（隣接攻撃、HP減少、撃破）
- 簡易テストマップの自動生成

## 起動手順（初心者向け）

1. Unity Hubでこのプロジェクトを開く
2. `Assets/Scenes/SampleScene.unity` を開く
3. Hierarchyで空のGameObjectを1つ作る
4. そのGameObjectに `GameBootstrap` をアタッチする
5. Playボタンを押す

## 開発ルール（安全に進めるため）

- `main` は常に安定状態を保つ
- 機能単位でブランチを切る
  - `feature/map-generation`
  - `feature/inventory`
  - `feature/ui-hud`
- コミットは小さく、意図が分かる単位で行う
- 手動プレイテスト後に `main` へマージする

## 次に実装する候補

1. 自動生成タイル描画をTilemapベースに置き換える
2. 複数敵と簡単なスポーンルールを追加する
3. 階段によるフロア遷移を追加する
4. 最小HUD（HP表示 + 戦闘ログ）を追加する
5. アイテム取得とインベントリを追加する
