# UnoTestApp02_Metronome
このプロジェクトは、Uno Platform & firebase hostingの学習プロジェクトです。

# Learned Points
・共有XAMLファイルを編集した後、Uno PlatformでUWPプロジェクトをデバッグしたい場合は、その前にUWPプロジェクトをビルドする必要がある。

・wasmプロジェクトでプラットフォーム固有の実装をする時、生成後のhtmlは直接いじれないので、jsでhtml要素を生成しなければならない。

・wasmプロジェクトでjsを呼ぶのはめちゃ簡単。

・wasmプロジェクトでjsからAssetのリソースにアクセスするには、js自身の所属フォルダを見つけて、そこから辿ればアクセスできる。(もっといい方法ないかなぁ？

・標準のタイマーは精度が低いので、音楽を扱うなら自前実装などが必要。

・

# Problems
・UWPnとWasmのプロジェクトしか実行できない。他のプロジェクトを実行するには？？

・Uno PlatformのプロジェクトでFirebaseとGitHubを連携させるには？(おそらく、GitHub ActionsでMSBuildを使用？)

・wasmプロジェクトが、画面を激しく動かすとちらつく。うぜー