import { defineConfig } from 'vitepress'
import vitepressProtectPlugin from "vitepress-protect-plugin"

// https://vitepress.dev/reference/site-config
export default defineConfig({
  lang: 'zh-CN',
  appearance:'dark', 
  head: [["link", {rel: "icon", href: "/RollCall-Website/RollCall.ico"}]],
  title: "RollCall | 帮助文档",
  description: "一款致力于上课点名时老师选择困难症的开源软件",
  base: '/RollCall-Website/', //网站部署的路径，默认根目录
  cleanUrls:true,
  lastUpdated: true, //首次配置不会立即生效，需git提交后爬取时间戳 //
  //markdown配置
  markdown: {
    // 组件插入h1标题下
    config: (md) => {
      md.renderer.rules.heading_close = (tokens, idx, options, env, slf) => {
          let htmlResult = slf.renderToken(tokens, idx, options);
          if (tokens[idx].tag === 'h1') htmlResult += `<ArticleMetadata />`; 
          return htmlResult;
      }
    },
    //中文配置
    container:{
      tipLabel: "提示",
      warningLabel: "警告",
      noteLabel: "注意",
      dangerLabel: "危险",
      detailsLabel: "详情",
      infoLabel: "信息",
    }
  },
  themeConfig: {
    // https://vitepress.dev/reference/default-theme-config
    logo: '/RollCall.ico',
    siteTitle: 'RollCall',
    nav: [
      { text: 'RollCall下载链接', link: 'https://github.com/MengdeUser/RollCall/releases' },
      { text: '帮助文档', link: '/about' },
    ],
    sidebar: [
      {
        text: 'RollCall帮助文档',
        collapsed: false,
        items: [
          { text: '概述', link: '/about' },
          { text: '安装与配置', link: '/download' },
          { text: '使用方法', link: '/run' },
          { text: '注意事项', link: '/info' },
          { text: '常见问题与解答', link: '/Q&A' },
          { text: '联系我们', link: '/call' },
        ]
      },
      {
        text: '感谢您使用RollCall！',
      }
    ],
    socialLinks: [
      { icon: 'github', link: 'https://github.com/MengdeUser/RollCall-Website/' }
    ],
    search: {
      provider: 'local',
      //配置中文提示
      options:{
        translations: {
          button: {
            buttonText: '搜索',
            buttonAriaLabel: '搜索'
          },
          modal: {
            resetButtonTitle: '清除查询条件',
            noResultsText: '没有找到结果',
            footer: {
              selectText: '选择',
              closeText: '关闭',
              navigateText: '导航到结果'
            }
          }
        }
      }
    },
    footer: {
      copyright: `©2024-${new Date().getFullYear()} By 拾光梦`,
    },
    //上次更新时间 //
    lastUpdated: {
      text: '最后更新于',
      formatOptions: {
        dateStyle: 'short', // 可选值full、long、medium、short
        timeStyle: 'medium' // 可选值full、long、medium、short
      },
    },
    //手机端配置返回顶部按钮显示文字
    returnToTopLabel: "返回顶部",
    //手机端配置侧边栏菜单按钮显示文字
    sidebarMenuLabel:'目录', 
    //右侧内容导航栏
    outline: {
      level: [2, 6],
      label:"导航"
    },
    //翻页
    docFooter: {
      prev: "上一页",
      next: "下一页",
    },
  },
  vite: {
    plugins: [
      vitepressProtectPlugin({
        disableF12: true, // 禁用F12开发者模式
        disableCopy: true, // 禁用文本复制
        disableSelect: true, // 禁用文本选择
      }),
    ],
  },
})
