USE [OtterDB]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 
GO
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [ImageUrl], [ImageLocalPath]) 
VALUES (1, N'Kimchi', N'https://localhost:7005/CategoryImages/1.png', N'wwwroot\CategoryImages\1.png'),
       (2, N'Jjigae', N'https://localhost:7005/CategoryImages/2.png', N'wwwroot\CategoryImages\2.png'),
       (3, N'Banchan', N'https://localhost:7005/CategoryImages/3.png', N'wwwroot\CategoryImages\3.png'),
       (4, N'Bulgogi and Gui', N'https://localhost:7005/CategoryImages/4.png', N'wwwroot\CategoryImages\4.png'),
       (5, N'Tteok', N'https://localhost:7005/CategoryImages/5.png', N'wwwroot\CategoryImages\5.png'),
       (6, N'Jeon', N'https://localhost:7005/CategoryImages/6.png', N'wwwroot\CategoryImages\6.png'),
       (7, N'Guksu', N'https://localhost:7005/CategoryImages/7.png', N'wwwroot\CategoryImages\7.png'),
       (8, N'Jjim', N'https://localhost:7005/CategoryImages/8.png', N'wwwroot\CategoryImages\8.png'),
       (9, N'Bibimbap', N'https://localhost:7005/CategoryImages/9.png', N'wwwroot\CategoryImages\9.png'),
       (10, N'Guksu', N'https://localhost:7005/CategoryImages/10.png', N'wwwroot\CategoryImages\10.png')
GO
