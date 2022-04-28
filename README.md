**Azure 学习验证**

# Azure Storage SAS Upload

### 运行截图

![image-20220428135815830](D:/learn/netcore/Hei.Azure.Test/images/README/image-20220428135815830.png)

### 流程



### 运行结果

> 注: 部分重复分片大小的测试是特地为之

网络状态不稳定下的结果

```
文件:20.5.mp4，大小：20.56(Mb)，切片大小:0.5(Mb),切片数:42 上传耗时：8.101(S)
文件:20.5.mp4，大小：20.56(Mb)，切片大小:1.0(Mb),切片数:21 上传耗时：5.817(S)
文件:20.5.mp4，大小：20.56(Mb)，切片大小:1.5(Mb),切片数:14 上传耗时：6.346(S)
文件:20.5.mp4，大小：20.56(Mb)，切片大小:2.0(Mb),切片数:11 上传耗时：7.199(S)
文件:20.5.mp4，大小：20.56(Mb)，切片大小:2.5(Mb),切片数:9 上传耗时：7.128(S)
文件:20.5.mp4，大小：20.56(Mb)，切片大小:3.0(Mb),切片数:7 上传耗时：7.26(S)
文件:20.5.mp4，大小：20.56(Mb)，切片大小:3.5(Mb),切片数:6 上传耗时：6.654(S)
文件:20.5.mp4，大小：20.56(Mb)，切片大小:4.0(Mb),切片数:6 上传耗时：7.168(S)
文件:20.5.mp4，大小：20.56(Mb)，切片大小:4.5(Mb),切片数:5 上传耗时：8.142(S)
文件:20.5.mp4，大小：20.56(Mb)，切片大小:5.0(Mb),切片数:5 上传耗时：7.789(S)
文件:20.5.mp4，大小：20.56(Mb)，切片大小:7.5(Mb),切片数:3 上传耗时：14.018(S)
文件:20.5.mp4，大小：20.56(Mb)，切片大小:10.0(Mb),切片数:3 上传耗时：8.737(S)

文件:52.mp4，大小：51.90(Mb)，切片大小:0.5(Mb),切片数:104 上传耗时：17.947(S)
文件:52.mp4，大小：51.90(Mb)，切片大小:2.0(Mb),切片数:26 上传耗时：16.437(S)
文件:52.mp4，大小：51.90(Mb)，切片大小:5.0(Mb),切片数:11 上传耗时：20.024(S)
文件:52.mp4，大小：51.90(Mb)，切片大小:7.5(Mb),切片数:7 上传耗时：20.586(S)
文件:52.mp4，大小：51.90(Mb)，切片大小:10(Mb),切片数:6 上传耗时：21.179(S)
文件:52.mp4，大小：51.90(Mb)，切片大小:1(Mb),切片数:52 上传耗时：18.185(S)
文件:52.mp4，大小：51.90(Mb)，切片大小:4.0(Mb),切片数:13 上传耗时：33.603(S)
文件:52.mp4，大小：51.90(Mb)，切片大小:4.0(Mb),切片数:13 上传耗时：21.91(S)

文件:101.mp4，大小：101.29(Mb)，切片大小:1(Mb),切片数:102 上传耗时：57.054(S)
文件:101.mp4，大小：101.29(Mb)，切片大小:2.0(Mb),切片数:51 上传耗时：34.811(S)
文件:101.mp4，大小：101.29(Mb)，切片大小:1.0(Mb),切片数:102 上传耗时：49.757(S)
文件:101.mp4，大小：101.29(Mb)，切片大小:2.0(Mb),切片数:51 上传耗时：32.112(S)
文件:101.mp4，大小：101.29(Mb)，切片大小:4.0(Mb),切片数:26 上传耗时：31.16(S)
文件:101.mp4，大小：101.29(Mb)，切片大小:5.0(Mb),切片数:21 上传耗时：35.192(S)
文件:101.mp4，大小：101.29(Mb)，切片大小:7.5(Mb),切片数:14 上传耗时：50.901(S)
文件:101.mp4，大小：101.29(Mb)，切片大小:5.0(Mb),切片数:21 上传耗时：28.2(S)
文件:101.mp4，大小：101.29(Mb)，切片大小:7.5(Mb),切片数:14 上传耗时：31.022(S)
```

尽量保证网络(限速5MB/s)下的结果：

```
文件:52.mp4，大小：51.90(MB)，切片大小:0.5(MB),切片数:104 上传耗时：12.199(S)；速率-->4.254 MB/s
文件:52.mp4，大小：51.90(MB)，切片大小:1.0(MB),切片数:52 上传耗时：11.056(S)；速率-->4.694 MB/s
文件:52.mp4，大小：51.90(MB)，切片大小:1.5(MB),切片数:35 上传耗时：11.354(S)；速率-->4.571 MB/s
文件:52.mp4，大小：51.90(MB)，切片大小:2.0(MB),切片数:26 上传耗时：11.066(S)；速率-->4.690 MB/s
文件:52.mp4，大小：51.90(MB)，切片大小:4.0(MB),切片数:13 上传耗时：11.074(S)；速率-->4.687 MB/s
文件:52.mp4，大小：51.90(MB)，切片大小:5.0(MB),切片数:11 上传耗时：11.102(S)；速率-->4.675 MB/s
文件:52.mp4，大小：51.90(MB)，切片大小:7.5(MB),切片数:7 上传耗时：11.217(S)；速率-->4.627 MB/s
文件:101.mp4，大小：101.29(MB)，切片大小:1.0(MB),切片数:102 上传耗时：21.161(S)；速率-->4.787 MB/s
文件:101.mp4，大小：101.29(MB)，切片大小:0.5(MB),切片数:203 上传耗时：23.268(S)；速率-->4.353 MB/s
文件:101.mp4，大小：101.29(MB)，切片大小:1.5(MB),切片数:68 上传耗时：21.575(S)；速率-->4.695 MB/s
文件:101.mp4，大小：101.29(MB)，切片大小:2(MB),切片数:51 上传耗时：21.178(S)；速率-->4.783 MB/s
文件:101.mp4，大小：101.29(MB)，切片大小:4(MB),切片数:26 上传耗时：21.177(S)；速率-->4.783 MB/s
文件:101.mp4，大小：101.29(MB)，切片大小:5(MB),切片数:21 上传耗时：21.26(S)；速率-->4.764 MB/s
文件:101.mp4，大小：101.29(MB)，切片大小:7.5(MB),切片数:14 上传耗时：21.304(S)；速率-->4.755 MB/s
文件:101.mp4，大小：101.29(MB)，切片大小:10(MB),切片数:11 上传耗时：21.403(S)；速率-->4.733 MB/s
文件:151.mp4，大小：151.77(MB)，切片大小:1.0(MB),切片数:152 上传耗时：31.541(S)；速率-->4.812 MB/s
文件:151.mp4，大小：151.77(MB)，切片大小:2.0(MB),切片数:76 上传耗时：31.466(S)；速率-->4.823 MB/s
文件:151.mp4，大小：151.77(MB)，切片大小:4.0(MB),切片数:38 上传耗时：31.477(S)；速率-->4.822 MB/s
文件:201.mp4，大小：201.19(MB)，切片大小:1.0(MB),切片数:202 上传耗时：41.608(S)；速率-->4.835 MB/s
文件:201.mp4，大小：201.19(MB)，切片大小:2.0(MB),切片数:101 上传耗时：41.552(S)；速率-->4.842 MB/s
文件:201.mp4，大小：201.19(MB)，切片大小:4.0(MB),切片数:51 上传耗时：41.561(S)；速率-->4.841 MB/s
```



### 结论

根据线速后上传的结果，可以得出结论：

**分片大小不会对文件上传速率有明显影响，分片大小影响的是网络请求次数**



