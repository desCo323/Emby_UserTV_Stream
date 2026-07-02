(function () {
  'use strict';

  function byId(id) {
    return document.getElementById(id);
  }

  function apiGet(path) {
    return ApiClient.getJSON(ApiClient.getUrl(path));
  }

  function apiPost(path, body) {
    return ApiClient.ajax({
      type: 'POST',
      url: ApiClient.getUrl(path),
      data: JSON.stringify(body || {}),
      contentType: 'application/json',
      dataType: 'json'
    });
  }

  function setValue(id, value) {
    const element = byId(id);
    if (element) {
      element.value = value == null ? '' : value;
    }
  }

  function setChecked(id, value) {
    const element = byId(id);
    if (element) {
      element.checked = !!value;
    }
  }

  function readInt(id, fallback) {
    const parsed = parseInt(byId(id).value, 10);
    return Number.isFinite(parsed) ? parsed : fallback;
  }

  function renderStatus(status) {
    byId('userTvRuntime').textContent = status.runtimeInitialized ? 'Initialized' : 'Not initialized';
    byId('userTvMode').textContent = status.mode || 'Unknown';
    if (status.lastRun) {
      byId('userTvLastRun').textContent = `${status.lastRun.finishedAt} | users ${status.lastRun.usersScanned} | eligible ${status.lastRun.eligibleUsers}`;
      byId('userTvOutput').textContent = JSON.stringify(status.lastRun, null, 2);
    }
  }

  function renderConfig(response) {
    const config = response.configuration || {};
    setChecked('enableAutomaticPlanning', config.enableAutomaticPlanning);
    setValue('planningIntervalMinutes', config.planningIntervalMinutes || 360);
    setChecked('enableExperimentalWrites', config.enableExperimentalWrites);
    setValue('playlistPrefix', config.playlistPrefix || 'UserTV');
    setValue('channelNameTemplate', config.channelNameTemplate || '{prefix} - {user}');
    setValue('minimumItemsPerUser', config.minimumItemsPerUser || 3);
    setValue('maximumItemsPerUser', config.maximumItemsPerUser || 250);
    setChecked('includeSeriesEpisodes', config.includeSeriesEpisodes !== false);
    setChecked('anonymizeDashboardExamples', config.anonymizeDashboardExamples !== false);
  }

  function readConfig() {
    return {
      enableAutomaticPlanning: byId('enableAutomaticPlanning').checked,
      planningIntervalMinutes: readInt('planningIntervalMinutes', 360),
      enableExperimentalWrites: byId('enableExperimentalWrites').checked,
      playlistPrefix: byId('playlistPrefix').value,
      channelNameTemplate: byId('channelNameTemplate').value,
      minimumItemsPerUser: readInt('minimumItemsPerUser', 3),
      maximumItemsPerUser: readInt('maximumItemsPerUser', 250),
      includeSeriesEpisodes: byId('includeSeriesEpisodes').checked,
      anonymizeDashboardExamples: byId('anonymizeDashboardExamples').checked
    };
  }

  function loadPage() {
    return Promise.all([
      apiGet('usertv/status').then(renderStatus),
      apiGet('usertv/configuration').then(renderConfig)
    ]);
  }

  window.ApiClientPlugin = window.ApiClientPlugin || {};
  window.ApiClientPlugin.userTv = { loadPage };

  document.addEventListener('viewshow', function (event) {
    if (!event.target || event.target.id !== 'userTvPluginPage') {
      return;
    }

    loadPage();

    byId('userTvRunDry').addEventListener('click', function () {
      apiPost('usertv/plan', { dryRun: true }).then(function (response) {
        byId('userTvOutput').textContent = JSON.stringify(response.summary || response, null, 2);
        return apiGet('usertv/status').then(renderStatus);
      });
    });

    byId('userTvConfigurationForm').addEventListener('submit', function (formEvent) {
      formEvent.preventDefault();
      apiPost('usertv/configuration', readConfig()).then(renderConfig);
    });
  });
})();
