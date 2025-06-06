USE [OtterDB]
GO
SET IDENTITY_INSERT [dbo].[OrderHeaders] ON 


INSERT [dbo].[OrderHeaders] ([OrderHeaderId], [UserId], [CouponCode], [Discount], [OrderTotal], [Name], [Phone], [Email], [OrderTime], [Status], [PaymentIntentId], [StripeSessionId]) 
VALUES 
--(1, N'4', N'10OFF', 10, 15.5, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2023-11-15T10:15:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
--(2, N'4', N'10OFF', 10, 19.99, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2023-12-10T12:30:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
--(3, N'4', N'10OFF', 10, 25.2, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-01-05T14:45:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
--(4, N'4', N'10OFF', 10, 32.75, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-02-20T09:50:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
--(5, N'4', N'10OFF', 10, 16, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-03-23T16:28:36.6922390' AS DateTime2), N'Completed', NULL, NULL),
--(6, N'4', N'10OFF', 10, 47.99, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-04-12T17:20:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
(7, N'4', N'10OFF', 10, 54.3, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-05-05T13:15:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
(8, N'4', N'10OFF', 10, 61.25, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-06-18T16:45:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
(9, N'4', N'10OFF', 10, 23.99, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-07-07T11:10:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
(10, N'4', N'10OFF', 10, 89.99, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-08-11T18:05:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
(11, N'4', N'10OFF', 10, 67.8, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-09-22T09:30:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
(12, N'4', N'10OFF', 10, 38.5, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-10-23T16:28:36.6922390' AS DateTime2), N'Completed', NULL, NULL),
(13, N'4', N'10OFF', 10, 28.5, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-10-16T11:20:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
(14, N'4', N'10OFF', 10, 45.75, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-10-17T14:10:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
(15, N'4', N'10OFF', 10, 33.99, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-10-18T09:50:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
(16, N'4', N'10OFF', 10, 76.2, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-10-19T16:45:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
(17, N'4', N'10OFF', 10, 51.0, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-10-20T12:30:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
(18, N'4', N'10OFF', 10, 62.5, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-10-21T10:15:00.0000000' AS DateTime2), N'Completed', NULL, NULL),
(19, N'4', N'10OFF', 10, 40.0, N'Minh Nhat Ho', N'0888408052', N'nhathcme170171@fpt.edu.com', CAST(N'2024-10-22T18:05:00.0000000' AS DateTime2), N'Completed', NULL, NULL)
SET IDENTITY_INSERT [dbo].[OrderHeaders] OFF
GO
