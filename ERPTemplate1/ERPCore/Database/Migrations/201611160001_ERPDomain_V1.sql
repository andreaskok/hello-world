GO
SET IDENTITY_INSERT [dbo].[DynamicMenu] ON 
GO
INSERT [dbo].[DynamicMenu] ([ID], [ParentID], [MenuLevel], [MenuName], [ControllerName], [MethodName], [AreaName], [PluginName], [Plugin], [Buy], [CreateDate], [UpdateDate]) VALUES (30, 5, 2, N'Trial Balance Report', N'TrialBalance', N'Index', N'PluginGL', N'', 1, 1, CAST(N'2016-11-04 00:00:00.000' AS DateTime), CAST(N'2016-11-04 00:00:00.000' AS DateTime))
GO
INSERT [dbo].[DynamicMenu] ([ID], [ParentID], [MenuLevel], [MenuName], [ControllerName], [MethodName], [AreaName], [PluginName], [Plugin], [Buy], [CreateDate], [UpdateDate]) VALUES (32, 5, 2, N'Profit And Loss Statement', N'ProfitAndLoss', N'Index', N'PluginGL', N'', 1, 1, CAST(N'2016-11-10 00:00:00.000' AS DateTime), CAST(N'2016-11-10 00:00:00.000' AS DateTime))
GO
INSERT [dbo].[DynamicMenu] ([ID], [ParentID], [MenuLevel], [MenuName], [ControllerName], [MethodName], [AreaName], [PluginName], [Plugin], [Buy], [CreateDate], [UpdateDate]) VALUES (33, 5, 2, N'Balance Sheet Statement', N'BalanceSheet', N'Index', N'PluginGL', N'', 1, 1, CAST(N'2016-11-15 00:00:00.000' AS DateTime), CAST(N'2016-11-15 00:00:00.000' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[DynamicMenu] OFF
GO
