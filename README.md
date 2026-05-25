# RocoKingdomEggQuery

洛克王国孵蛋查询器 - WPF 应用程序

## 功能介绍

- 查询洛克王国宠物孵蛋信息
- 支持多种筛选条件
- 基于贝叶斯模型的智能推荐
- 友好的用户界面

## 技术栈

- **语言**: C#
- **框架**: .NET 10.0 / WPF
- **模型**: 贝叶斯分类器

## 项目结构

```
RocoKingdomEggQuery/
├── Commands/          # 命令模式实现
├── Converters/        # 数据绑定转换器
├── Data/              # 数据文件
├── Models/            # 数据模型
├── Properties/        # 项目属性
├── Resources/         # 资源文件
├── Services/          # 服务层
├── ViewModels/        # 视图模型
├── Views/             # 视图页面
├── App.xaml           # 应用程序入口
└── MainWindow.xaml    # 主窗口
```

## 使用方法

### 开发环境

1. 克隆仓库到本地
2. 使用 Visual Studio 2022 或更高版本打开解决方案
3. 还原 NuGet 依赖
4. 编译并运行

### 运行方式

```bash
# 克隆仓库
git clone https://github.com/yourusername/RocoKingdomEggQuery.git

# 进入项目目录
cd RocoKingdomEggQuery

# 使用 Visual Studio 打开解决方案
start RocoKingdomEggQuery.slnx
```

## 贡献

欢迎提交 Issue 和 Pull Request！

## 许可证

MIT License
