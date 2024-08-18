﻿using Microsoft.Extensions.Options;
using MediaBase.Models;
using MediaBase.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediaBase.Services
{
    public class MovieRequestManager : RequestManagerBase<Movie>
    {
        public MovieRequestManager(MoviePathProvider pathProvider, IOptions<MediaConfigs> config)
            : base(pathProvider, config.Value.MimeTypes) { }

        public FileStreamResult GetStream(string title, int year)
        {
            var movieFileInfos = pathProvider.CollectMediaInfos().Select(x => (MovieFileInfo)x);
            var movieFileInfo = movieFileInfos.FirstOrDefault(x => x.Title == title && x.Year == year);

            if (movieFileInfo == null)
                throw new ArgumentException($"Movie not found based on the specified params! - Title: {title} | Year: {year}");

            return GetStream(movieFileInfo.FilePath, movieFileInfo.Extension);
        }

        protected override IEnumerable<Movie> GetMedias(IList<IMediaFileInfo> mediaFileInfos)
        {
            return mediaFileInfos.Select(x => ((MovieFileInfo) x).Get());
        }
    }
}