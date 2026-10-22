<script lang="ts">
    import TodoList from '../components/TodoList.svelte';
import todoList from '../components/TodoList.svelte'
</script>

<main>
    <h1>Svelte Todo</h1>

    <!-- ADD NEW TASK -->
    <div class="addTodoContainer">
        <div>
            <label for="addtodo">New task</label>
        </div>
        <div class="d-flex flex-column flex-gap">
            <input type="text" id="addtodo" class="textInput">
            <button class="primaryButton">Add</button>
        </div>
    </div>

    <!-- DISPLAY TASKS -->
    <div class="todoTaskContainer">
        <h2>Tasks</h2>
        <article class="todoTask">
            <p>Task text</p>
            <button class="primaryButton">Update</button>
            <button class="primaryButton">Delete</button>
        </article>
    </div>
</main>

<TodoList></TodoList>

<style>
    :global(body) {
        background-color: #1e1e2e;
        color: white;
        margin: 0;
        padding: 0;
    }

    main{
        padding: 1rem;
    }

    .d-flex{
        display: flex;
    }

    .flex-column{
        flex-direction: column;
    }
    .flex-gap{
        gap: 0.25rem;
    }
    
    
    .addTodoContainer{
        border: 1px solid #7d7f81;
        padding: 1rem;
        background: #29323b;
        margin-bottom: 1rem;

        border-radius: 5px;
    }

    .todoTaskContainer{
        border: 1px solid #7d7f81;
        border-radius: 5px;
        padding: 1rem;
        background: #29323b;
    }
    .todoTask{
        border: 1px solid #7d7f81;
        background: #3d4a57;
        border-radius: 10px;
        padding: 1rem;
    }

    .primaryButton{
        border-radius: 5px;
        padding: 0.25rem;
        background: #7d7f81;
        color: white;
    }

    .textInput{
        padding-top: 0.5rem;
        padding-bottom: 0.5rem;
    }
</style>