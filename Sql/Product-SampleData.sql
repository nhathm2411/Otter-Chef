USE [OtterDB]
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProductId], [Name], [Price], [Description], [CategoryId], [ImageUrl], [ImageLocalPath]) 
VALUES (1, N'Baechu Kimchi', 5, N'Traditional napa cabbage kimchi, made with salted cabbage, chili flakes, garlic, and fermented seafood.', 1, N'https://localhost:7000/ProductImages/1.png', N'wwwroot\ProductImages\1.png'),
       (2, N'Kkakdugi', 5, N'Cubed radish kimchi, crunchy and spicy, usually served with soups and stews.', 1, N'https://localhost:7000/ProductImages/2.png', N'wwwroot\ProductImages\2.png'),
       (3, N'Baek Kimchi', 6, N'White kimchi, a non-spicy version made without chili pepper, offering a mild and refreshing flavor.', 1, N'https://localhost:7000/ProductImages/3.png', N'wwwroot\ProductImages\3.png'),
       (4, N'Kimchi Jjigae', 8, N'Spicy kimchi stew, typically made with aged kimchi, pork, tofu, and vegetables, simmered in a rich broth.', 2, N'https://localhost:7000/ProductImages/4.png', N'wwwroot\ProductImages\4.png'),
       (5, N'Doenjang Jjigae', 7, N'Fermented soybean paste stew with vegetables, tofu, and sometimes clams or anchovies, offering a savory and umami-rich flavor.', 2, N'https://localhost:7000/ProductImages/5.png', N'wwwroot\ProductImages\5.png'),
       (6, N'Sundubu Jjigae', 8, N'Soft tofu stew with seafood or pork, featuring a spicy broth and silky soft tofu.', 2, N'https://localhost:7000/ProductImages/6.png', N'wwwroot\ProductImages\6.png'),
       (7, N'Japchae', 7, N'Stir-fried glass noodles with vegetables, soy sauce, and sesame oil, often served as a side dish or appetizer.', 3, N'https://localhost:7000/ProductImages/7.png', N'wwwroot\ProductImages\7.png'),
       (8, N'Kongnamul Muchim', 3, N'Seasoned bean sprout salad, lightly blanched and tossed with sesame oil, garlic, and salt.', 3, N'https://localhost:7000/ProductImages/8.png', N'wwwroot\ProductImages\8.png'),
       (9, N'Kimchi Bokkeum', 5, N'Stir-fried kimchi, often with pork or tuna, giving a smoky and intense flavor.', 3, N'https://localhost:7000/ProductImages/9.png', N'wwwroot\ProductImages\9.png'),
       (10, N'Bulgogi', 12, N'Thinly sliced marinated beef, grilled or stir-fried, served with rice and various side dishes.', 4, N'https://localhost:7000/ProductImages/10.png', N'wwwroot\ProductImages\10.png'),
       (11, N'Samgyeopsal', 10, N'Grilled pork belly, typically enjoyed with lettuce wraps, garlic, and ssamjang (a savory dipping sauce).', 4, N'https://localhost:7000/ProductImages/11.png', N'wwwroot\ProductImages\11.png'),
       (12, N'Galbi', 15, N'Marinated beef short ribs, either grilled (Galbi Gui) or braised (Galbi Jjim), known for their sweet and savory flavor.', 4, N'https://localhost:7000/ProductImages/12.png', N'wwwroot\ProductImages\12.png'),
       (13, N'Tteokbokki', 7, N'Spicy stir-fried rice cakes in a gochujang-based sauce, often served with fish cakes and boiled eggs.', 5, N'https://localhost:7000/ProductImages/13.png', N'wwwroot\ProductImages\13.png'),
       (14, N'Gungjung Tteokbokki', 8, N'Royal court-style Tteokbokki, featuring a mild soy-based sauce with beef and vegetables.', 5, N'https://localhost:7000/ProductImages/14.png', N'wwwroot\ProductImages\14.png'),
       (15, N'Injeolmi', 6, N'Soft rice cakes coated with roasted soybean powder, offering a chewy texture and nutty flavor.', 5, N'https://localhost:7000/ProductImages/15.png', N'wwwroot\ProductImages\15.png'),
       (16, N'Haemul Pajeon', 12, N'Savory pancake made with green onions and mixed seafood, served with a soy-vinegar dipping sauce.', 6, N'https://localhost:7000/ProductImages/16.png', N'wwwroot\ProductImages\16.png'),
       (17, N'Kimchi Jeon', 7, N'Kimchi pancake, made with fermented kimchi, flour, and sometimes added pork or seafood.', 6, N'https://localhost:7000/ProductImages/17.png', N'wwwroot\ProductImages\17.png'),
       (18, N'Bindaetteok', 9, N'Mung bean pancake with ground mung beans, vegetables, and pork, fried until crispy on the outside.', 6, N'https://localhost:7000/ProductImages/18.png', N'wwwroot\ProductImages\18.png'),
       (19, N'Naengmyeon', 11, N'Cold buckwheat noodles, typically served in a chilled broth (Mul Naengmyeon) or with a spicy sauce (Bibim Naengmyeon).', 7, N'https://localhost:7000/ProductImages/19.png', N'wwwroot\ProductImages\19.png'),
       (20, N'Jajangmyeon', 6, N'Noodles in black bean sauce with diced pork and vegetables, known for its rich and slightly sweet flavor.', 7, N'https://localhost:7000/ProductImages/20.png', N'wwwroot\ProductImages\20.png'),
       (21, N'Kalguksu', 14, N'Hand-cut wheat noodles served in a clear broth, usually made with chicken or seafood.', 7, N'https://localhost:7000/ProductImages/21.png', N'wwwroot\ProductImages\21.png'),
       (22, N'Galbi-jjim', 20, N'Braised beef short ribs with soy sauce, sugar, and vegetables, known for its tender meat and sweet-savory flavor.', 8, N'https://localhost:7000/ProductImages/22.png', N'wwwroot\ProductImages\22.png'),
       (23, N'Dakbokkeumtang', 24, N'Spicy braised chicken with potatoes and carrots, cooked in a gochujang-based sauce.', 8, N'https://localhost:7000/ProductImages\23.png', N'wwwroot\ProductImages\23.png'),
       (24, N'Haemul-jjim', 30, N'Spicy braised seafood, often featuring squid, clams, and shrimp, simmered in a gochugaru (red chili) sauce.', 8, N'https://localhost:7000/ProductImages\24.png', N'wwwroot\ProductImages\24.png'),
       (25, N'Jeonju Bibimbap', 10, N'A specialty of Jeonju city, featuring rice topped with assorted vegetables, a fried egg, and gochujang (spicy chili paste).', 9, N'https://localhost:7000/ProductImages\25.png', N'wwwroot\ProductImages\25.png'),
       (26, N'Dolsot Bibimbap', 16, N'Bibimbap served in a hot stone bowl, which crisps the rice on the bottom for added texture.', 9, N'https://localhost:7000/ProductImages\26.png', N'wwwroot\ProductImages\26.png'),
       (27, N'Yukhoe Bibimbap', 20, N'Bibimbap with seasoned raw beef (yukhoe) and raw egg, offering a unique savory and umami taste.', 9, N'https://localhost:7000/ProductImages\27.png', N'wwwroot\ProductImages\27.png'),
       (28, N'Jokbal', 28, N'Braised pig''s trotters seasoned with soy sauce, garlic, and ginger, served with lettuce and dipping sauces.', 10, N'https://localhost:7000/ProductImages\28.png', N'wwwroot\ProductImages\28.png'),
       (29, N'Budae Jjigae', 25, N'"Army stew" made with a mix of kimchi, Spam, sausages, tofu, and ramen noodles, developed after the Korean War.', 10, N'https://localhost:7000/ProductImages\29.png', N'wwwroot\ProductImages\29.png'),
       (30, N'Dakbal', 8, N'Spicy grilled or braised chicken feet, often marinated in a hot chili sauce and served as a bar snack.', 10, N'https://localhost:7000/ProductImages\30.png', N'wwwroot\ProductImages\30.png')

SET IDENTITY_INSERT [dbo].[Products] OFF
GO
