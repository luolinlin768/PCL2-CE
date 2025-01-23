Class PageLogsLeft

#Region "页面切换"

    ''' <summary>
    ''' 当前页面的编号。从 0 开始计算。
    ''' </summary>
    Public PageID As FormMain.PageSubType = FormMain.PageSubType.Default


    Public Function PageGet(Optional ID As FormMain.PageSubType = -1)
        If ID = -1 Then ID = PageID
        Select Case ID
            Case FormMain.PageSubType.VersionOverall
                If FrmVersionOverall Is Nothing Then FrmVersionOverall = New PageVersionOverall
                Return FrmVersionOverall
            Case FormMain.PageSubType.VersionMod
                If FrmVersionMod Is Nothing Then FrmVersionMod = New PageVersionMod
                Return FrmVersionMod
            Case FormMain.PageSubType.VersionModDisabled
                If FrmVersionModDisabled Is Nothing Then FrmVersionModDisabled = New PageVersionModDisabled
                Return FrmVersionModDisabled
            Case FormMain.PageSubType.VersionSetup
                If IsNothing(FrmVersionSetup) Then FrmVersionSetup = New PageVersionSetup
                Return FrmVersionSetup
            Case FormMain.PageSubType.VersionWorld
                If FrmVersionWorld Is Nothing Then FrmVersionWorld = New PageVersionWorld
                Return FrmVersionWorld
            Case FormMain.PageSubType.VersionScreenshot
                If FrmVersionScreenshot Is Nothing Then FrmVersionScreenshot = New PageVersionScreenshot
                Return FrmVersionScreenshot
            Case FormMain.PageSubType.VersionResourcePack
                If FrmVersionResourcePack Is Nothing Then FrmVersionResourcePack = New PageVersionResourcePack
                Return FrmVersionResourcePack
            Case FormMain.PageSubType.VersionShader
                If FrmVersionShader Is Nothing Then FrmVersionShader = New PageVersionShader
                Return FrmVersionShader
            Case Else
                Throw New Exception("未知的版本设置子页面种类：" & ID)
        End Select
    End Function

    ''' <summary>
    ''' 切换现有页面。
    ''' </summary>
    Public Sub PageChange(ID As FormMain.PageSubType)
        If PageID = ID Then Exit Sub
        AniControlEnabled += 1
        Try
            PageChangeRun(PageGet(ID))
            PageID = ID
        Catch ex As Exception
            Log(ex, "切换分页面失败（ID " & ID & "）", LogLevel.Feedback)
        Finally
            AniControlEnabled -= 1
        End Try
    End Sub
    Private Shared Sub PageChangeRun(Target As MyPageRight)
        AniStop("FrmMain PageChangeRight") '停止主页面的右页面切换动画，防止它与本动画一起触发多次 PageOnEnter
        If Target.Parent IsNot Nothing Then Target.SetValue(ContentPresenter.ContentProperty, Nothing)
        FrmMain.PageRight = Target
        CType(FrmMain.PanMainRight.Child, MyPageRight).PageOnExit()
        AniStart({
        AaCode(Sub()
                   CType(FrmMain.PanMainRight.Child, MyPageRight).PageOnForceExit()
                   FrmMain.PanMainRight.Child = FrmMain.PageRight
                   FrmMain.PageRight.Opacity = 0
               End Sub, 130),
        AaCode(Sub()
                   '延迟触发页面通用动画，以使得在 Loaded 事件中加载的控件得以处理
                   FrmMain.PageRight.Opacity = 1
                   FrmMain.PageRight.PageOnEnter()
               End Sub, 30, True)
    }, "PageLeft PageChange")
    End Sub

#End Region

    Public Sub Refresh(sender As Object, e As EventArgs) '由边栏按钮匿名调用
        Select Case Val(sender.Tag)
            Case FormMain.PageSubType.VersionMod
                PageVersionMod.Refresh()
            Case FormMain.PageSubType.VersionScreenshot
                PageVersionScreenshot.Refresh()
            Case FormMain.PageSubType.VersionWorld
                PageVersionWorld.Refresh()
            Case FormMain.PageSubType.VersionResourcePack
                PageVersionResourcePack.Refresh()
            Case FormMain.PageSubType.VersionShader
                PageVersionShader.Refresh()
        End Select
    End Sub


End Class