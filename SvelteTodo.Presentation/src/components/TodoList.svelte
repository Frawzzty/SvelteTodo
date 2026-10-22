<script lang="ts">
  import { onMount } from 'svelte';

  type TodoTask = {
    id: string;
    text: string;
    isDone: boolean;
    doneByDate: string;
  };

  let tasks = $state<TodoTask[]>([]);
  let loading = $state(true);
  let error = $state('');

  onMount(async () => {
    try {
      const response = await fetch(
        'https://localhost:7190/api/tasks'
      );

      if (!response.ok) {
        throw new Error(`HTTP error: ${response.status}`);
      }

      tasks = await response.json();
    } catch (err) {
      error = err instanceof Error
        ? err.message
        : 'Failed to load tasks';
    } finally {
      loading = false;
    }
  });
</script>

<h1>My Todo Tasks</h1>

{#if loading}
  <p>Loading tasks...</p>
{:else if error}
  <p>Error: {error}</p>
{:else}
  <ul>
    {#each tasks as task (task.id)}
      <li>
        <input type="checkbox" checked={task.isDone} disabled />
        <span>{task.text}</span>
        <small> — Due: {task.doneByDate}</small>
      </li>
    {/each}
  </ul>
{/if}