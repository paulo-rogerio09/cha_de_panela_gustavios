import { useEffect, useRef } from 'react';
import Matter from 'matter-js';

function App() {
  const sceneRef = useRef(null);

  useEffect(() => {
    // 1. Inicializar as ferramentas do Matter.js
    const { Engine, Render, Runner, Bodies, Composite } = Matter;

    const engine = Engine.create();

    // 2. Configurar o "palco" (canvas) para ocupar o ecrã todo
    const render = Render.create({
      element: sceneRef.current,
      engine: engine,
      options: {
        width: window.innerWidth,
        height: window.innerHeight,
        wireframes: false,
        background: '#fdf2f8' // Fundo rosa claro para o Chá de Panela
      }
    });

    // 3. Criar o chão invisível para as bolhas não caírem para fora do ecrã
    const chao = Bodies.rectangle(
        window.innerWidth / 2,
        window.innerHeight + 25,
        window.innerWidth,
        50,
        { isStatic: true }
    );

    // 4. Criar as nossas primeiras bolhas de teste
    const bolha1 = Bodies.circle(window.innerWidth / 2 - 50, 50, 50, {
      render: { fillStyle: '#f472b6' },
      restitution: 0.8 // Faz a bolha saltar um pouco
    });

    const bolha2 = Bodies.circle(window.innerWidth / 2 + 60, -100, 60, {
      render: { fillStyle: '#fb7185' },
      restitution: 0.9
    });

    // 5. Adicionar tudo ao mundo e ligar o motor
    Composite.add(engine.world, [chao, bolha1, bolha2]);

    Render.run(render);
    const runner = Runner.create();
    Runner.run(runner, engine);

    // 6. Limpeza quando o componente for desmontado
    return () => {
      Render.stop(render);
      Runner.stop(runner);
      Engine.clear(engine);
      if (render.canvas) render.canvas.remove();
    };
  }, []);

  return (
      <div style={{ position: 'relative', overflow: 'hidden', height: '100vh' }}>
        <div ref={sceneRef} />

        {/* Título sobreposto à física */}
        <h1 style={{
          position: 'absolute',
          top: '40px',
          left: '50%',
          transform: 'translateX(-50%)',
          color: '#831843',
          fontFamily: 'sans-serif',
          pointerEvents: 'none' // Para o rato não interagir com o texto e sim com as bolhas
        }}>
          Chá de Panela
        </h1>
      </div>
  );
}

export default App;